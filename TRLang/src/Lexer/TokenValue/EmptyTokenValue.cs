using System;

namespace TRLang.src.Lexer.TokenValue
{
    class EmptyTokenValue : TokenValue
    {
        public byte Value
        {
            get
            {
                throw new Exception("Empty token accessed.");
            }

            private set { }
        }

        public override string ToString() => "<Empty Token Value>";
    }
}
