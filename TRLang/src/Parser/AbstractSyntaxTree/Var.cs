using TRLang.src.Lexer;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Var : AstNode
    {
        public string Name { get; private set; }
        public Token Token { get; private set; }

        public Var(Token token)
        {
            this.Token = token;

            if (token.Value is CharTokenValue c) this.Name = c.Value.ToString();
            else this.Name = ((StringTokenValue)token.Value).Value;
        }

        public override string ToString() => $"Var(Name={this.Name})";
    }
}
