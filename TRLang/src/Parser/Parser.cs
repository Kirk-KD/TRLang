using System;
using System.Collections.Generic;
using TRLang.src.Error;
using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;
using TRLang.src.Parser.AbstractSyntaxTree;

namespace TRLang.src.Parser
{
    class Parser
    {
        private readonly Lexer.Lexer _lexer;
        private Token _currentToken;

        public Parser(Lexer.Lexer lexer)
        {
            this._lexer = lexer;
            this._currentToken = this._lexer.GetNextToken();
        }

        private void Eat(TokenType tokenType)
        {
            this.Log($"TokenExpectation: Expected={tokenType}, Actual={this._currentToken.Type}");

            if (this._currentToken.IsType(tokenType)) this._currentToken = this._lexer.GetNextToken();
            else this.Error(ErrorCode.UnexpectedToken, this._currentToken, $"Expected a token of type {tokenType}");
        }

        public AstNode Parse()
        {
            return this.Program();
        }

        /*
        * GRAMMARS
        *
        * program              : statement
        * compound_statement   : L_CURLY statement_list R_CURLY
        * statement_list       : statement | statement SEMI statement_list
        * statement            : compound_statement | assignment_statement | decl_statement | empty
        * variable_decl        : type_spec ID (ASSIGN expr)
        * type_spec            : INT_TYPE | FLOAT_TYPE
        * assignment_statement : variable ASSIGN expr
        * var_decl_statement   : var_decl_list
        * var_decl_list        : VAR variable_decl (COMMA variable_decl)*
        * formal_param         : ID COLON type_spec
        * formal_param_list    : formal_param (COMA formal_param)*
        * function_decl        : FUNC ID LROUND formal_param_list RROUND statement
        * func_decl_statement  : function_decl
        * empty                :
        * expr                 : term ((PLUS | MINUS) term)*
        * term                 : factor ((MUL | DIV) factor)*
        * factor               : PLUS factor
        *                      | MINUS factor
        *                      | INT
        *                      | FLOAT
        *                      | L_ROUND expr R_ROUND
        *                      | variable
        * variable             : ID
        */

        private AstNode Factor()
        {
            switch (this._currentToken.Type)
            {
                case TokenType.Int:
                    Int intNode = new Int(_currentToken);
                    this.Eat(TokenType.Int);
                    return intNode;

                case TokenType.Float:
                    Float floatNode = new Float(_currentToken);
                    this.Eat(TokenType.Float);
                    return floatNode;

                case TokenType.LRound:
                    this.Eat(TokenType.LRound);
                    AstNode node = this.Expr();
                    this.Eat(TokenType.RRound);

                    return node;

                case TokenType.Plus:
                    this.Eat(TokenType.Plus);
                    return new UnaryOp(this._currentToken, this.Factor());

                case TokenType.Minus:
                    this.Eat(TokenType.Minus);
                    return new UnaryOp(this._currentToken, this.Factor());

                default: return this.Variable();
            }
        }

        private AstNode Term()
        {
            AstNode node = this.Factor();

            while (this._currentToken.IsType(TokenType.Mul) || this._currentToken.IsType(TokenType.Div))
            {
                Token tok = this._currentToken;
                this.Eat(tok.Type);
                node = new BinOp(node, tok, this.Factor());
            }

            return node;
        }

        private AstNode Expr()
        {
            AstNode node = this.Term();

            while (this._currentToken.IsType(TokenType.Plus) || this._currentToken.IsType(TokenType.Minus))
            {
                Token tok = this._currentToken;
                this.Eat(this._currentToken.Type);
                node = new BinOp(node, tok, this.Term());
            }

            return node;
        }

        private AstNode Empty() => new NoOp();

        private AstNode TypeSpec()
        {
            Token tok = this._currentToken;

            if (this._currentToken.IsType(TokenType.IntType)) this.Eat(TokenType.IntType);
            else this.Eat(TokenType.FloatType);

            return new TypeSpec(tok);
        }

        private AstNode Variable()
        {
            AstNode node = new Var(this._currentToken);
            this.Eat(TokenType.Id);

            return node;
        }

        private AstNode VariableDecl()
        {
            AstNode varNode = this.Variable();
            this.Eat(TokenType.Colon);
            AstNode typeNode = this.TypeSpec();

            AstNode valueNode = null;
            if (this._currentToken.IsType(TokenType.Assign))
            {
                this.Eat(TokenType.Assign);
                valueNode = this.Expr();
            }

            return new VarDecl(varNode, typeNode, valueNode);
        }

        private AstNode VariableAssignmentStatement()
        {
            AstNode leftNode = this.Variable();
            Token tok = this._currentToken;
            this.Eat(TokenType.Assign);
            AstNode rightNode = this.Expr();

            return new Assign(leftNode, tok, rightNode);
        }

        private List<AstNode> VariableDeclList()
        {
            this.Eat(TokenType.Var);
            List<AstNode> varDeclNodes = new List<AstNode> { this.VariableDecl() };

            while (this._currentToken.IsType(TokenType.Comma))
            {
                this.Eat(TokenType.Comma);
                varDeclNodes.Add(this.VariableDecl());
            }

            return varDeclNodes;
        }

        private List<AstNode> FormalParamList()
        {
            List<AstNode> paramList = new List<AstNode>();

            if (!this._currentToken.IsType(TokenType.Id)) return paramList;

            do
            {
                AstNode var = this.Variable();
                this.Eat(TokenType.Colon);
                AstNode type = this.TypeSpec();

                paramList.Add(new Param(var, type));
            }
            while (this._currentToken.IsType(TokenType.Comma));

            return paramList;
        }

        private AstNode FunctionDecl()
        {
            this.Eat(TokenType.Func);

            string funcName = ((StringTokenValue)this._currentToken.Value).Value;
            this.Eat(TokenType.Id);

            this.Eat(TokenType.LRound);
            List<AstNode> paramList = this.FormalParamList();
            this.Eat(TokenType.RRound);

            AstNode funcBody = this.Statement();

            return new FuncDecl(funcName, paramList, funcBody);
        }

        private AstNode VariableDeclStatement()
        {
            List<AstNode> nodes = this.VariableDeclList();
            Compound root = new Compound();

            foreach (AstNode node in nodes) root.Children.Add(node);

            return root;
        }

        private AstNode FunctionDeclStatement() => this.FunctionDecl();

        private List<AstNode> StatementList()
        {
            List<AstNode> statements = new List<AstNode> { this.Statement() };

            while (this._currentToken.IsType(TokenType.Semi))
            {
                this.Eat(TokenType.Semi);
                statements.Add(this.Statement());
            }

            return statements;
        }

        private AstNode CompoundStatement()
        {
            this.Eat(TokenType.LCurly);
            List<AstNode> nodes = this.StatementList();
            this.Eat(TokenType.RCurly);

            Compound root = new Compound();
            foreach (AstNode node in nodes) root.Children.Add(node);

            return root;
        }

        private AstNode Statement()
        {
            return this._currentToken.Type switch
            {
                TokenType.LCurly => this.CompoundStatement(),
                TokenType.Id => this.VariableAssignmentStatement(),
                TokenType.Var => this.VariableDeclStatement(),
                TokenType.Func => this.FunctionDeclStatement(),
                _ => this.Empty()
            };
        }

        private AstNode Program()
        {
            List<AstNode> nodes = this.StatementList();
            if (!this._currentToken.IsType(TokenType.Eof)) this.Error(ErrorCode.UnexpectedToken, this._currentToken, "End-of-File expected.");

            return new Program(nodes);
        }

        private void Error(ErrorCode err, Token token, string details = null)
        {
            throw new ParserError($"{err} caused by {token}" + (details != null ? $" ({details})" : String.Empty));
        }

        private void Log(string message)
        {
            if (Flags.LogParser) Console.WriteLine($"Parser: {message}");
        }
    }
}
