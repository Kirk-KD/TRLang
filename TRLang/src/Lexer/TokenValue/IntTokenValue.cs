namespace TRLang.src.Lexer.TokenValue
{
    class IntTokenValue : TokenValue
    {
        public int Value { get; private set; }

        public IntTokenValue(int value)
        {
            this.Value = value;
        }

        public override string ToString() => this.Value.ToString();
    }
}
