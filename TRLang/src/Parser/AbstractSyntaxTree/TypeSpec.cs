using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRLang.src.Lexer;

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
