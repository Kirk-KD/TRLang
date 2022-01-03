using System;
using System.Collections.Generic;
using TRLang.src.Lexer;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class FuncCall : AstNode
    {
        public string FuncName { get; private set; }
        public List<AstNode> ActualParams { get; private set; }
        public Token Token { get; private set; }

        public FuncCall(string funcName, List<AstNode> actualParams, Token token)
        {
            this.FuncName = funcName;
            this.ActualParams = actualParams;
            this.Token = token;
        }

        public override string ToString() => $"FuncCall(FuncName={this.FuncName}, ActualParams=[{String.Join(", ", this.ActualParams)}])";
    }
}
