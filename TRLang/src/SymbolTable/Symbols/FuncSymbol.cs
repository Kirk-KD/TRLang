using System;
using System.Collections.Generic;

namespace TRLang.src.SymbolTable.Symbols
{
    class FuncSymbol : Symbol
    {
        public List<VarSymbol> Params { get; private set; }

        public FuncSymbol(string name, List<VarSymbol> paramList = null) : base(name)
        {
            this.Params = paramList ?? new List<VarSymbol>();
        }

        public override string ToString() => $"FuncSymbol(Name={this.Name}, Params=[{(this.Params != null ? String.Join(", ", this.Params) : "")}])";
    }
}
