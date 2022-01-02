using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.SymbolTable.Symbols
{
    class BuiltinTypeSymbol : Symbol
    {
        public BuiltinTypeSymbol(string name) : base(name) { }

        public override string ToString() => $"BuiltinTypeSymbol(Name={this.Name})";
    }
}
