using System;
using System.Collections.Generic;
using TRLang.src.Parser.AbstractSyntaxTree;

namespace TRLang.src.SymbolTable.Symbols
{
    public class FuncSymbol : Symbol
    {
        public List<VarSymbol> Params { get; private set; }
        public AstNode Body { get; private set; }

        public FuncSymbol(string name, List<VarSymbol> paramList = null) : base(name)
        {
            this.Params = paramList ?? new List<VarSymbol>();
        }

        public void SetBody(AstNode node)
        {
            this.Body = node;
        }

        public override string ToString() => $"FuncSymbol(Name={this.Name}, Params=[{(this.Params != null ? string.Join(", ", this.Params) : "")}])";
    }
}
