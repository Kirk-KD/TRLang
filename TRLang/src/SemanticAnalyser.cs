using System;
using TRLang.src.Error;
using TRLang.src.Lexer;
using TRLang.src.Parser.AbstractSyntaxTree;
using TRLang.src.SymbolTable;
using TRLang.src.SymbolTable.Symbols;

namespace TRLang.src
{
    class SemanticAnalyser : AstNodeVisitor<Void>
    {
        public ScopedSymbolTable CurrentScope { get; private set; }
        public AstNode RootNode { get; private set; }

        public SemanticAnalyser(AstNode node)
        {
            this.RootNode = node;
        }

        public void Analyse()
        {
            ScopedSymbolTable builtinsScope = new ScopedSymbolTable("<BUILTINS>", 0);
            builtinsScope.InitBuiltins();

            LogEnterScope(builtinsScope);
            this.CurrentScope = builtinsScope;

            this.GenericVisit(this.RootNode);

            LogLeaveScope(this.CurrentScope);
            this.CurrentScope = this.CurrentScope.EnclosingScope;
        }

        protected override void BeforeVisit(AstNode node)
        {
            Log($"Visit: {node}");
        }

        protected override Void Visit(Int node) => new Void();

        protected override Void Visit(Float node) => new Void();

        protected override Void Visit(BinOp node)
        {
            this.GenericVisit(node.LeftNode);
            this.GenericVisit(node.RightNode);

            return new Void();
        }

        protected override Void Visit(UnaryOp node)
        {
            this.GenericVisit(node.ExprNode);

            return new Void();
        }

        protected override Void Visit(Compound node)
        {
            foreach (AstNode n in node.Children) this.GenericVisit(n);

            return new Void();
        }

        protected override Void Visit(NoOp node) => new Void();

        protected override Void Visit(Assign node)
        {
            this.GenericVisit(node.LeftNode);
            this.GenericVisit(node.RightNode);

            return new Void();
        }

        protected override Void Visit(Var node)
        {
            if (this.CurrentScope.Lookup(node.Name) == null)
                Error(ErrorCode.IdNotFound, node.Token);

            return new Void();
        }

        protected override Void Visit(TypeSpec node) => new Void();

        protected override Void Visit(VarDecl node)
        {
            string typeName = ((TypeSpec)node.TypeNode).TypeName;
            BuiltinTypeSymbol typeSymbol = (BuiltinTypeSymbol)this.CurrentScope.Lookup(typeName);
            string varName = ((Var)node.VarNode).Name;

            if (this.CurrentScope.Lookup(varName, curr_scope_only: true) != null)
                Error(ErrorCode.DuplicateId, ((Var)node.VarNode).Token);

            VarSymbol varSymbol = new VarSymbol(varName, typeSymbol);
            this.CurrentScope.Insert(varSymbol);

            if (node.ValueNode != null) this.GenericVisit(node.ValueNode);

            return new Void();
        }

        protected override Void Visit(FuncDecl node)
        {
            FuncSymbol funcSymbol = new FuncSymbol(node.FuncName);
            this.CurrentScope.Insert(funcSymbol);

            ScopedSymbolTable funcScope = new ScopedSymbolTable(node.FuncName, this.CurrentScope.ScopeLevel + 1, this.CurrentScope);

            LogEnterScope(funcScope);
            this.CurrentScope = funcScope;

            foreach (AstNode n in node.Params)
            {
                Param param = (Param)n;

                BuiltinTypeSymbol type = (BuiltinTypeSymbol)this.CurrentScope.Lookup(((TypeSpec)(param.TypeNode)).TypeName);
                string name = ((Var)(param.VarNode)).Name;
                VarSymbol var = new VarSymbol(name, type);

                this.CurrentScope.Insert(var);
                funcSymbol.Params.Add(var);
            }

            this.GenericVisit(node.BodyNode);

            LogLeaveScope(funcScope);
            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return new Void();
        }

        protected override Void Visit(FuncCall node)
        {
            if (node.ActualParams.Count != ((FuncSymbol)this.CurrentScope.Lookup(node.FuncName)).Params.Count)
                Error(ErrorCode.IncorrectParamsCount, node.Token);

            foreach (AstNode paramNode in node.ActualParams) this.GenericVisit(paramNode);

            return new Void();
        }

        protected override Void Visit(Program node)
        {
            ScopedSymbolTable globalScope = new ScopedSymbolTable("<GLOBAL>", 1, this.CurrentScope); // this.CurrentScope is the builtins scope.

            LogEnterScope(globalScope);
            this.CurrentScope = globalScope;

            foreach (AstNode n in node.Nodes) this.GenericVisit(n);

            LogLeaveScope(this.CurrentScope);
            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return new Void();
        }

        private static void LogLeaveScope(ScopedSymbolTable scope)
        {
            if (Flags.LogSymbolTables) Console.WriteLine(scope);
            Log($"Leave Scope: {scope.ScopeName}, Level={scope.ScopeLevel}");
        }

        private static void LogEnterScope(ScopedSymbolTable scope)
        {
            Log($"Enter Scope: {scope.ScopeName}, Level={scope.ScopeLevel}");
        }

        private static void Error(ErrorCode err, Token token)
        {
            throw new SemanticError($"{err} caused by {token}");
        }

        private static void Log(string message)
        {
            if (Flags.LogSemanticAnalyser) Console.WriteLine($"SemanticAnalyser: {message}");
        }
    }
}
