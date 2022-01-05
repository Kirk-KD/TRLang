namespace TRLang.src.SymbolTable.Symbols
{
    public abstract class Symbol
    {
        public string Name { get; private set; }

        public Symbol(string name)
        {
            this.Name = name;
        }

        public abstract override string ToString();
    }
}
