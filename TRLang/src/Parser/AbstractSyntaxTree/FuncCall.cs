using System;
using System.Collections.Generic;
using TRLang.src.Lexer;
using TRLang.src.SymbolTable.Symbols;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class FuncCall : AstNode
    {
        public string FuncName { get; private set; }
        public List<AstNode> ActualParams { get; private set; }
        public Token Token { get; private set; }
        public FuncSymbol FuncSymbol { get; private set; }

        public FuncCall(string funcName, List<AstNode> actualParams, Token token)
        {
            this.FuncName = funcName;
            this.ActualParams = actualParams;
            this.Token = token;
        }

        public void SetFuncSymbol(FuncSymbol funcSymbol)
        {
            this.FuncSymbol = funcSymbol;
        }

        public override string ToString() => $"FuncCall(FuncName={this.FuncName}, ActualParams=[{string.Join(", ", this.ActualParams)}])";
    }
}
