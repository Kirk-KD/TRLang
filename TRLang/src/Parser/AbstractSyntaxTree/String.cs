using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class String : AstNode
    {
        public string Value { get; private set; }

        public String(Token token)
        {
            this.Value = ((StringTokenValue)token.Value).Value;
        }

        public override string ToString() => $"String(Value=\"{this.Value}\")";
    }
}
