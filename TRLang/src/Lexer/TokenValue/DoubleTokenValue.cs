namespace TRLang.src.Lexer.TokenValue
{
    class DoubleTokenValue : TokenValue
    {
        public double Value { get; private set; }

        public DoubleTokenValue(double value)
        {
            this.Value = value;
        }

        public override string ToString() => this.Value.ToString();
    }
}
