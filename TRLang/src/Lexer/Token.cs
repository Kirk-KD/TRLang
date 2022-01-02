using System;
using System.Collections.Generic;
using System.Text;

namespace TRLang.src.Lexer
{
    enum TokenType
    {
        Int,
        Float,

        Plus,
        Minus,
        Mul,
        Div,
        Assign,

        LRound,
        RRound,
        LCurly,
        RCurly,

        Semi,
        Colon,
        Comma,
        Dot,

        IntType,
        FloatType,

        Id,
        Eof,
        
        Empty,

        // Reserved keywords
        Main,
        Var,
        Func
    }

    class Token
    {
        public TokenType Type { get; private set; }
        public TokenValue Value { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }
        
        public Token()
        {
            this.Type = TokenType.Empty;
            this.Value = new EmptyTokenValue();
        }

        public Token(TokenType type, int line, int column)
        {
            this.Type = type;
            this.Value = new EmptyTokenValue();
            this.Line = line;
            this.Column = column;
        }

        public Token(TokenType tokenType, TokenValue tokenValue, int line, int column)
        {
            this.Type = tokenType;
            this.Value = tokenValue;
            this.Line = line;
            this.Column = column;
        }

        public bool IsType(TokenType type) => this.Type == type;

        public override string ToString() => $"Token(Type={this.Type}, Value={this.Value}, Position={this.Line}:{this.Column})";
    }
}
