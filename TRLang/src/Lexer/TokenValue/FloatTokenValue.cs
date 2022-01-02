namespace TRLang.src.Lexer.TokenValue
{
    class FloatTokenValue : TokenValue
    {
        public float Value { get; private set; }

        public FloatTokenValue(float value)
        {
            this.Value = value;
        }

        public override string ToString() => this.Value.ToString();
    }
}
