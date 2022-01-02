using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.SymbolTable.Symbols
{
    abstract class Symbol
    {
        public string Name { get; private set; }

        public Symbol(string name)
        {
            this.Name = name;
        }

        public abstract override string ToString();
    }
}
