using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRLang.src.Lexer;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Int : AstNode
    {
        public int Value { get; private set; }

        public Int(Token token)
        {
            this.Value = ((IntTokenValue)token.Value).Value;
        }

        public override string ToString() => $"Int(Value={this.Value})";
    }
}
