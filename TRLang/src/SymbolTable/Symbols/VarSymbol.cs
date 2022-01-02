namespace TRLang.src.SymbolTable.Symbols
{
    class VarSymbol : Symbol
    {
        public BuiltinTypeSymbol Type { get; private set; }

        public VarSymbol(string name, BuiltinTypeSymbol type) : base(name)
        {
            this.Type = type;
        }

        public override string ToString() => $"VarSymbol(Name={this.Name}, Type={this.Type})";
    }
}
