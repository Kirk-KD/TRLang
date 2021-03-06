using System;
using System.Collections.Generic;
using TRLang.src.Error;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Lexer
{
    public class Lexer
    {
        private readonly string _text;
        private int _pos = 0;
        private int _line = 1;
        private int _column = 1;

        public char CurrentChar { get; private set; }

        private static readonly Dictionary<string, TokenType> ReservedKeywords = new Dictionary<string, TokenType>
        {
            // Reserved Keywords
            { "var", TokenType.Var },
            { "func", TokenType.Func },

            // Datatypes
            { "int", TokenType.IntType },
            { "dbl", TokenType.DoubleType },
            { "str", TokenType.StringType }
        };

        public Lexer(string text)
        {
            this._text = text;
            this.CurrentChar = this._pos < this._text.Length ? this._text[this._pos] : Char.MinValue;

            Log($"Character: '{this.CurrentChar}' at {this._line}:{this._column}");
        }

        private void Advance()
        {
            if (this.CurrentChar == '\n')
            {
                this._line++;
                this._column = 0;
            }

            this._pos++;

            if (this._pos < this._text.Length)
            {
                this.CurrentChar = this._text[this._pos];
                this._column++;
            }
            else this.CurrentChar = Char.MinValue;

            Log($"Character: '{Utilities.SmartCharToString(this.CurrentChar)}' at {this._line}:{this._column}");
        }

        private void SkipComment()
        {
            while (this.CurrentChar != Char.MinValue && this.CurrentChar != '\n') this.Advance();
            this.Advance();
        }

        private void SkipWhitespace()
        {
            while (this.CurrentChar != Char.MinValue && Char.IsWhiteSpace(this.CurrentChar)) this.Advance();
        }

        private Token MakeNumber()
        {
            string result = String.Empty;

            while (this.CurrentChar != Char.MinValue && Char.IsDigit(this.CurrentChar))
            {
                result += this.CurrentChar;
                this.Advance();
            }

            if (this.CurrentChar == '.')
            {
                result += this.CurrentChar;
                this.Advance();

                while (this.CurrentChar != Char.MinValue && Char.IsDigit(this.CurrentChar))
                {
                    result += this.CurrentChar;
                    this.Advance();
                }

                return new Token(TokenType.Double, new DoubleTokenValue(Double.Parse(result)), this._line, this._column);
            }

            return new Token(TokenType.Int, new IntTokenValue(Int32.Parse(result)), this._line, this._column);
        }

        private Token MakeId()
        {
            string result = String.Empty;

            while (this.CurrentChar != Char.MinValue && (Char.IsLetterOrDigit(this.CurrentChar) || this.CurrentChar == '_'))
            {
                result += this.CurrentChar;
                this.Advance();
            }

            return new Token(
                ReservedKeywords.ContainsKey(result) ? ReservedKeywords[result] : TokenType.Id,
                new StringTokenValue(result), this._line, this._column);
        }

        private Token MakeString()
        {
            this.Advance(); // Skip beginning quote

            string result = String.Empty;

            while (this.CurrentChar != '"')
            {
                if ("\r\n\0".Contains(this.CurrentChar)) Error();

                result += this.CurrentChar;
                this.Advance();
            }

            this.Advance();

            return new Token(TokenType.String, new StringTokenValue(result), this._line, this._column);
        }

        public Token GetNextToken()
        {
            while (this.CurrentChar != Char.MinValue)
            {
                if (Char.IsWhiteSpace(this.CurrentChar))
                {
                    this.SkipWhitespace();
                    continue;
                }
                else if (this.CurrentChar == '#')
                {
                    this.Advance();
                    this.SkipComment();
                    continue;
                }
                else if (this.CurrentChar == '"') return this.MakeString();
                else if (Char.IsDigit(this.CurrentChar)) return this.MakeNumber();
                else if (Char.IsLetter(this.CurrentChar)) return this.MakeId();

                char currentChar = this.CurrentChar;
                this.Advance();

                TokenType type = currentChar switch
                {
                    '+' => TokenType.Plus,
                    '-' => TokenType.Minus,
                    '*' => TokenType.Mul,
                    '/' => TokenType.Div,
                    '(' => TokenType.LRound,
                    ')' => TokenType.RRound,
                    '{' => TokenType.LCurly,
                    '}' => TokenType.RCurly,
                    '=' => TokenType.Assign,
                    ';' => TokenType.Semi,
                    ':' => TokenType.Colon,
                    ',' => TokenType.Comma,
                    '.' => TokenType.Dot,
                    _ => TokenType.Empty
                };

                if (type == TokenType.Empty) this.Error();

                return new Token(type, new CharTokenValue(currentChar), this._line, this._column);
            }

            return new Token(TokenType.Eof, this._line, this._column);
        }

        private void Error()
        {
            throw new LexerError($"Unexpected character '{this.CurrentChar}' at line {this._line}, column {this._column}");
        }

        private static void Log(string message)
        {
            if (Flags.LogLexer) Console.WriteLine($"Lexer: {message}");
        }
    }
}
