using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Lexer.TokenValue
{
    class StringTokenValue : TokenValue
    {
        public string Value { get; private set; }

        public StringTokenValue(string value)
        {
            this.Value = value;
        }

        public override string ToString() => $"\"{this.Value}\"";
    }
}
