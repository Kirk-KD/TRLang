using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Lexer
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public TokenValue.TokenValue Value { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }

        public Token()
        {
            this.Type = TokenType.Empty;
            this.Value = new EmptyTokenValue();
        }

        public Token(TokenType type, int line, int column)
        {
            this.Type = type;
            this.Value = new EmptyTokenValue();
            this.Line = line;
            this.Column = column;
        }

        public Token(TokenType tokenType, TokenValue.TokenValue tokenValue, int line, int column)
        {
            this.Type = tokenType;
            this.Value = tokenValue;
            this.Line = line;
            this.Column = column;
        }

        public bool IsType(TokenType type) => this.Type == type;

        public bool IsEndOfStmtList() =>
            this.IsType(TokenType.Empty) ||
            this.IsType(TokenType.RCurly) ||
            this.IsType(TokenType.Eof);

        public Token Clone() => new Token(this.Type, this.Value, this.Line, this.Column);

        public override string ToString() => $"Token(Type={this.Type}, Value={this.Value}, Position={this.Line}:{this.Column})";
    }
}
