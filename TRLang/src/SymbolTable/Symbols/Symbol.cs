﻿namespace TRLang.src.SymbolTable.Symbols
{
    public abstract class Symbol
    {
        public string Name { get; private set; }
        public int Level { get; set; }

        public Symbol(string name)
        {
            this.Name = name;
        }

        public abstract override string ToString();
    }
}
