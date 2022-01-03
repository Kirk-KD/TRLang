using System;
using TRLang.src.Parser.AbstractSyntaxTree;
using TRLang.src.CallStack;
using TRLang.src.SymbolTable.Symbols;
using System.Collections.Generic;

namespace TRLang.src.Interpreter
{
    class Interpreter : AstNodeVisitor<InterpreterVisitResult>
    {
        private readonly AstNode RootNode;
        public readonly CallStack.CallStack CallStack = new CallStack.CallStack();

        public Interpreter(AstNode node)
        {
            this.RootNode = node;
        }

        public InterpreterVisitResult Interpret() => this.GenericVisit(this.RootNode);

        protected override void BeforeVisit(AstNode node) => Log($"Visit: {node.GetTypeName()}");

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

                    default: Error(); return new InterpreterVisitResult();
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

                    default: Error(); return new InterpreterVisitResult();
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

                default: Error(); return new InterpreterVisitResult();
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

            ActivationRecord ar = this.CallStack.Peek();
            ar.Set(key, result.HasValue(result.FloatValue) ? (float)result.FloatValue : (int)result.IntValue);

            return new InterpreterVisitResult();
        }

        protected override InterpreterVisitResult Visit(Var node)
        {
            ActivationRecord ar = this.CallStack.Peek();

            if (ar.ContainsKey(node.Name))
            {
                if (ar.Get(node.Name) is int value) return new InterpreterVisitResult(value);
                else return new InterpreterVisitResult((float)ar.Get(node.Name));
            }
            else
            {
                Error();
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

                ActivationRecord ar = this.CallStack.Peek();
                ar.Set(key, result.HasValue(result.FloatValue) ? (float)result.FloatValue : (int)result.IntValue);
            }

            return new InterpreterVisitResult();
        }

        protected override InterpreterVisitResult Visit(FuncDecl node) => new InterpreterVisitResult();

        protected override InterpreterVisitResult Visit(FuncCall node)
        {
            string funcName = node.FuncName;

            ActivationRecord ar = new ActivationRecord(funcName, ARType.Function, 2);

            FuncSymbol funcSymbol = node.FuncSymbol;

            List<VarSymbol> formal = funcSymbol.Params;
            List<AstNode> actual = node.ActualParams;

            for (int i = 0; i < formal.Count; i++)
            {
                InterpreterVisitResult result = this.GenericVisit(actual[i]);

                if (result.HasValue(result.IntValue)) ar.Set(formal[i].Name, result.IntValue);
                else if (result.HasValue(result.FloatValue)) ar.Set(formal[i].Name, result.FloatValue);
                else Error();
            }

            this.CallStack.Push(ar);

            this.GenericVisit(funcSymbol.Body);

            this.LogCallStack();

            this.CallStack.Pop();

            return new InterpreterVisitResult();
        }

        protected override InterpreterVisitResult Visit(Program node)
        {
            ActivationRecord ar = new ActivationRecord("<GLOBAL>", ARType.Program, 1);
            this.CallStack.Push(ar);

            foreach (AstNode n in node.Nodes) this.GenericVisit(n);

            this.LogCallStack();

            this.CallStack.Pop();

            return new InterpreterVisitResult();
        }

        private static void Error()
        {
            throw new Exception("ERROR IN INTERPRETER SHOULD NOT BE POSSIBLE.");
        }

        private static void Log(string message)
        {
            if (Flags.LogInterpreter) Console.WriteLine($"Interpreter: {message}");
        }

        private void LogCallStack()
        {
            if (Flags.LogCallStack) Console.WriteLine(this.CallStack);
        }
    }
}
