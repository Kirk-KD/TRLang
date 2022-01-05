using System;
using TRLang.src.Parser.AbstractSyntaxTree;
using TRLang.src.CallStack;
using TRLang.src.SymbolTable.Symbols;
using System.Collections.Generic;
using TRLang.src.Lexer;

namespace TRLang.src
{
    public class Interpreter : AstNodeVisitor<object>
    {
        private readonly AstNode RootNode;
        public readonly CallStack.CallStack CallStack = new CallStack.CallStack();
        public readonly List<ActivationRecord> ArHistory = new List<ActivationRecord>();

        public Interpreter(AstNode node)
        {
            this.RootNode = node;
        }

        public object Interpret() => this.GenericVisit(this.RootNode);

        protected override void BeforeVisit(AstNode node) => Log($"Visit: {node.GetTypeName()}");

        protected override object Visit(Int node) => node.Value;

        protected override object Visit(Float node) => node.Value;

        protected override object Visit(BinOp node)
        {
            object leftResult = this.GenericVisit(node.LeftNode);
            object rightResult = this.GenericVisit(node.RightNode);

            bool leftIsInt = leftResult is int;
            bool rightIsInt = rightResult is int;

            int? intLeft = leftIsInt ? (int)leftResult : null;
            float? floatLeft = !leftIsInt ? (float)leftResult : null;
            int? intRight = rightIsInt ? (int)rightResult : null;
            float? floatRight = !rightIsInt ? (float)rightResult : null;

            float? result = node.Op.Type switch
            {
                TokenType.Plus => (intLeft ?? floatLeft) + (intRight ?? floatRight),
                TokenType.Minus => (intLeft ?? floatLeft) - (intRight ?? floatRight),
                TokenType.Mul => (intLeft ?? floatLeft) * (intRight ?? floatRight),
                TokenType.Div => (intLeft ?? floatLeft) / (intRight ?? floatRight),

                _ => null
            };

            if (result == null)
            {
                Error();
                return null;
            }

            if (leftIsInt && rightIsInt && !node.Op.IsType(TokenType.Div))
            {
                return Convert.ToInt32(result!);
            }
            else return (result!);
        }

        protected override object Visit(UnaryOp node)
        {
            object exprResult = this.GenericVisit(node.ExprNode);

            float result;
            switch (node.Op.Type)
            {
                case TokenType.Minus: result = -Convert.ToSingle(exprResult); break;
                case TokenType.Plus: result = Convert.ToSingle(exprResult); break;

                default: Error(node.Op.Type.ToString()); return null;
            }

            if (exprResult is int) return Convert.ToInt32(result);
            else return result;
        }

        protected override object Visit(Compound node)
        {
            foreach (var child in node.Children) this.GenericVisit(child);

            return null;
        }

        protected override object Visit(NoOp node) => null;

        protected override object Visit(Assign node)
        {
            string key = ((Var)node.LeftNode).Name;
            object result = this.GenericVisit(node.RightNode);

            ActivationRecord ar = this.CallStack.Peek();
            ar.Set(key, result);

            return null;
        }

        protected override object Visit(Var node)
        {
            ActivationRecord ar = this.CallStack.Peek();

            if (ar.ContainsKey(node.Name)) return ar.Get(node.Name);
            else
            {
                Error();
                return null;
            }
        }

        protected override object Visit(TypeSpec node) => null;

        protected override object Visit(VarDecl node)
        {
            if (node.ValueNode != null)
            {
                string key = ((Var)node.VarNode).Name;
                object result = this.GenericVisit(node.ValueNode);

                ActivationRecord ar = this.CallStack.Peek();
                ar.Set(key, result);
            }

            return null;
        }

        protected override object Visit(FuncDecl node) => null;

        protected override object Visit(FuncCall node)
        {
            string funcName = node.FuncName;

            ActivationRecord ar = new ActivationRecord(funcName, ARType.Function, 2);

            FuncSymbol funcSymbol = node.FuncSymbol;

            List<VarSymbol> formal = funcSymbol.Params;
            List<AstNode> actual = node.ActualParams;

            for (int i = 0; i < formal.Count; i++)
            {
                object result = this.GenericVisit(actual[i]);
                ar.Set(formal[i].Name, result);
            }

            this.CallStack.Push(ar);

            this.GenericVisit(funcSymbol.Body);

            this.LogCallStack();

            this.ArHistory.Insert(0, this.CallStack.Pop());

            return null;
        }

        protected override object Visit(Program node)
        {
            ActivationRecord ar = new ActivationRecord("<GLOBAL>", ARType.Program, 1);
            this.CallStack.Push(ar);

            foreach (AstNode n in node.Nodes) this.GenericVisit(n);

            this.LogCallStack();

            this.ArHistory.Insert(0, this.CallStack.Pop());

            return null;
        }

        private static void Error(string details = "No details")
        {
            throw new Exception($"ERROR IN INTERPRETER SHOULD NOT BE POSSIBLE ({details})");
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
