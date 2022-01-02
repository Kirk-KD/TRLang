using System;
using System.Collections.Generic;
using System.Text;

namespace TRLang.src.Lexer
{
    abstract class TokenValue
    {
        public abstract override string ToString();
    }

    class EmptyTokenValue : TokenValue {
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

    class IntTokenValue : TokenValue
    {
        public int Value { get; private set; }

        public IntTokenValue(int value)
        {
            this.Value = value;
        }

        public override string ToString() => this.Value.ToString();
    }

    class FloatTokenValue : TokenValue
    {
        public float Value { get; private set; }

        public FloatTokenValue(float value)
        {
            this.Value = value;
        }

        public override string ToString() => this.Value.ToString();
    }

    class StringTokenValue : TokenValue
    {
        public string Value { get; private set; }

        public StringTokenValue(string value)
        {
            this.Value = value;
        }

        public override string ToString() => $"\"{this.Value}\"";
    }

    class CharTokenValue : TokenValue
    {
        public char Value { get; private set; }

        public CharTokenValue(char value)
        {
            this.Value = value;
        }

        public override string ToString() => $"'{this.Value}'";

        public StringTokenValue ToStringTokenValue() => new StringTokenValue(this.Value.ToString());
    }
}
