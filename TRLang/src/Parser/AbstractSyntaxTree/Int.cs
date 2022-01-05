using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class Int : AstNode
    {
        public int Value { get; private set; }

        public Int(Token token)
        {
            this.Value = ((IntTokenValue)token.Value).Value;
        }

        public override string ToString() => $"Int(Value={this.Value})";
    }
}
