using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRLang.src.Lexer;

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
