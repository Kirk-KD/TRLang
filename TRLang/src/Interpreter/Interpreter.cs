using System;
using System.Collections.Generic;
using TRLang.src.Parser.AbstractSyntaxTree;

namespace TRLang.src.Interpreter
{
    class Interpreter : AstNodeVisitor<InterpreterVisitResult>
    {
        private readonly AstNode RootNode;
        public Dictionary<string, object> GlobalMemory { get; private set; } = new Dictionary<string, object>();

        public Interpreter(AstNode node)
        {
            this.RootNode = node;
        }

        public InterpreterVisitResult Interpret() => this.GenericVisit(this.RootNode);

        protected override void BeforeVisit(AstNode node) => this.Log($"Visit: {node.GetTypeName()}");

        protected override InterpreterVisitResult Visit(Int node) => new InterpreterVisitResult(node.Value);

        protected override InterpreterVisitResult Visit(Float node) => new InterpreterVisitResult(node.Value);

        protected override InterpreterVisitResult Visit(BinOp node)
        {
            InterpreterVisitResult leftResult = this.GenericVisit(node.LeftNode);
            InterpreterVisitResult rightResult = this.GenericVisit(node.RightNode);

            bool returnFloat = leftResult.HasValue(leftResult.FloatValue) || rightResult.HasValue(rightResult.FloatValue);

            if (returnFloat)
            {
                // Convert value to float
                float left = (node.LeftNode is Int) ? ((Int)node.LeftNode).Value : ((Float)node.LeftNode).Value;
                float right = (node.RightNode is Int) ? ((Int)node.RightNode).Value : ((Float)node.RightNode).Value;

                switch (node.Op.Type)
                {
                    case Lexer.TokenType.Plus: return new InterpreterVisitResult(left + right);
                    case Lexer.TokenType.Minus: return new InterpreterVisitResult(left - right);
                    case Lexer.TokenType.Mul: return new InterpreterVisitResult(left * right);
                    case Lexer.TokenType.Div: return new InterpreterVisitResult(left / right);

                    default: this.Error(); return new InterpreterVisitResult();
                }
            }
            else
            {
                // Convert value to int
                int left = ((Int)node.LeftNode).Value;
                int right = ((Int)node.RightNode).Value;

                switch (node.Op.Type)
                {
                    case Lexer.TokenType.Plus: return new InterpreterVisitResult(left + right);
                    case Lexer.TokenType.Minus: return new InterpreterVisitResult(left - right);
                    case Lexer.TokenType.Mul: return new InterpreterVisitResult(left * right);
                    case Lexer.TokenType.Div: return new InterpreterVisitResult(left / right);

                    default: this.Error(); return new InterpreterVisitResult();
                }
            }
        }

        protected override InterpreterVisitResult Visit(UnaryOp node)
        {
            InterpreterVisitResult exprResult = this.GenericVisit(node.ExprNode);

            switch (node.Op.Type)
            {
                case Lexer.TokenType.Minus:
                    return exprResult.HasValue(exprResult.FloatValue) ?
                        new InterpreterVisitResult(-(float)exprResult.FloatValue) :
                        new InterpreterVisitResult(-(int)exprResult.IntValue);
                case Lexer.TokenType.Plus:
                    return exprResult.HasValue(exprResult.FloatValue) ?
                        new InterpreterVisitResult((float)exprResult.FloatValue) :
                        new InterpreterVisitResult((int)exprResult.IntValue);

                default: this.Error(); return new InterpreterVisitResult();
            }
        }

        protected override InterpreterVisitResult Visit(Compound node)
        {
            foreach (var child in node.Children) this.GenericVisit(child);

            return new InterpreterVisitResult();
        }

        protected override InterpreterVisitResult Visit(NoOp node) => new InterpreterVisitResult();

        protected override InterpreterVisitResult Visit(Assign node)
        {
            string key = ((Var)node.LeftNode).Name;
            InterpreterVisitResult result = this.GenericVisit(node.RightNode);

            this.GlobalMemory[key] = result.HasValue(result.FloatValue) ? (float)result.FloatValue : (int)result.IntValue;

            return new InterpreterVisitResult();
        }

        protected override InterpreterVisitResult Visit(Var node)
        {
            if (this.GlobalMemory.ContainsKey(node.Name))
            {
                if (this.GlobalMemory[node.Name] is int value) return new InterpreterVisitResult(value);
                else return new InterpreterVisitResult((float)this.GlobalMemory[node.Name]);
            }
            else
            {
                this.Error();
                return new InterpreterVisitResult();
            }
        }

        protected override InterpreterVisitResult Visit(TypeSpec node) => new InterpreterVisitResult();

        protected override InterpreterVisitResult Visit(VarDecl node)
        {
            if (node.ValueNode != null)
            {
                string key = ((Var)node.VarNode).Name;
                InterpreterVisitResult result = this.GenericVisit(node.ValueNode);

                this.GlobalMemory[key] = result.HasValue(result.FloatValue) ? (float)result.FloatValue : (int)result.IntValue;
            }

            return new InterpreterVisitResult();
        }

        protected override InterpreterVisitResult Visit(FuncDecl node) => new InterpreterVisitResult();

        protected override InterpreterVisitResult Visit(Program node)
        {
            foreach (AstNode n in node.Nodes) this.GenericVisit(n);

            return new InterpreterVisitResult();
        }

        private void Error()
        {
            throw new Exception("ERROR IN INTERPRETER SHOULD NOT BE POSSIBLE.");
        }

        private void Log(string message)
        {
            if (Flags.LogInterpreter) Console.WriteLine($"Interpreter: {message}");
        }
    }
}
