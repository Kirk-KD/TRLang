using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Float : AstNode
    {
        public float Value { get; private set; }

        public Float(Token token)
        {
            this.Value = ((FloatTokenValue)token.Value).Value;
        }

        public override string ToString() => $"Float(Value={this.Value})";
    }
}
