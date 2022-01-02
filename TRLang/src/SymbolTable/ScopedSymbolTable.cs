using System;
using System.Collections.Generic;
using TRLang.src.SymbolTable.Symbols;

namespace TRLang.src.SymbolTable
{
    class ScopedSymbolTable
    {
        private readonly Dictionary<string, Symbol> _symbols = new Dictionary<string, Symbol>();
        public string ScopeName { get; private set; }
        public int ScopeLevel { get; private set; }
        public ScopedSymbolTable EnclosingScope { get; private set; }

        public ScopedSymbolTable(string scopeName, int scopeLevel, ScopedSymbolTable enclosingScope = null)
        {
            this.ScopeName = scopeName;
            this.ScopeLevel = scopeLevel;
            this.EnclosingScope = enclosingScope;
        }

        public void InitBuiltins()
        {
            this.Insert(new BuiltinTypeSymbol("int"));
            this.Insert(new BuiltinTypeSymbol("float"));
        }

        public void Insert(Symbol symbol)
        {
            this.Log($"Insert: {symbol} (Scope={this.ScopeName}, Level={this.ScopeLevel})");
            this._symbols[symbol.Name] = symbol;
        }

#nullable enable
        public Symbol? Lookup(string name, bool curr_scope_only = false)
        {
            this.Log($"Lookup: {name} (Scope={this.ScopeName}, Level={this.ScopeLevel})");
            Symbol? symbol = this._symbols.GetValueOrDefault(name);

            if (symbol != null) return symbol;

            if (curr_scope_only) return null;

            if (this.EnclosingScope != null) return this.EnclosingScope.Lookup(name);

            return null;
        }
#nullable disable

        public override string ToString()
        {
            string result = $"ScopedSymbolTable(Scope={this.ScopeName}, Level={this.ScopeLevel}) Content:";

            foreach (KeyValuePair<string, Symbol> pair in this._symbols)
                result += $"\n  {pair.Key} = {pair.Value}";

            return result;
        }

        private void Log(string message)
        {
            if (Flags.LogSymbolTables) Console.WriteLine($"ScopedSymbolTable: {message}");
        }
    }
}
