using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class TypeSpec : AstNode
    {
        public string TypeName { get; private set; }

        public TypeSpec(Token token)
        {
            this.TypeName = ((StringTokenValue)token.Value).Value;
        }

        public override string ToString() => $"Type(TypeName={this.TypeName})";
    }
}
