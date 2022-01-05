namespace TRLang.src.SymbolTable.Symbols
{
    public class BuiltinTypeSymbol : Symbol
    {
        public BuiltinTypeSymbol(string name) : base(name) { }

        public override string ToString() => $"BuiltinTypeSymbol(Name={this.Name})";
    }
}
