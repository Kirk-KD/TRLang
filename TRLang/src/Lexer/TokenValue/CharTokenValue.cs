namespace TRLang.src.Lexer.TokenValue
{
    class CharTokenValue : TokenValue
    {
        public char Value { get; private set; }

        public CharTokenValue(char value)
        {
            this.Value = value;
        }

        public override string ToString() => $"'{this.Value}'";

        public StringTokenValue ToStringTokenValue() => new StringTokenValue(this.Value.ToString());
    }
}
