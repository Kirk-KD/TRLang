using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class Double : AstNode
    {
        public double Value { get; private set; }

        public Double(Token token)
        {
            this.Value = ((DoubleTokenValue)token.Value).Value;
        }

        public override string ToString() => $"Double(Value={this.Value})";
    }
}
