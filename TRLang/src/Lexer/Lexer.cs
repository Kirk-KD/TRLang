using System;
using System.Collections.Generic;
using TRLang.src.Error;
using TRLang.src.Lexer.TokenValue;

namespace TRLang.src.Lexer
{
    class Lexer
    {
        private readonly string _text;
        private int _pos = 0;
        private int _line = 1;
        private int _column = 1;
        private char _currentChar;

        private static readonly Dictionary<string, TokenType> ReservedKeywords = new Dictionary<string, TokenType>
        {
            // Reserved Keywords
            { "main", TokenType.Main },
            { "var", TokenType.Var },
            { "func", TokenType.Func },

            // Datatypes
            { "int", TokenType.IntType },
            { "float", TokenType.FloatType }
        };

        public Lexer(string text)
        {
            this._text = text;
            this._currentChar = this._pos < this._text.Length ? this._text[this._pos] : Char.MinValue;

            this.Log($"Character: '{this._currentChar}' at {this._line}:{this._column}");
        }

        private void Advance()
        {
            if (this._currentChar == '\n')
            {
                this._line++;
                this._column = 0;
            }

            this._pos++;

            if (this._pos < this._text.Length)
            {
                this._currentChar = this._text[this._pos];
                this._column++;
            }
            else this._currentChar = Char.MinValue;

            this.Log($"Character: '{Utilities.SmartCharToString(this._currentChar)}' at {this._line}:{this._column}");
        }

        private void SkipComment()
        {
            while (this._currentChar != Char.MinValue && this._currentChar != '\n') this.Advance();
            this.Advance();
        }

        private void SkipWhitespace()
        {
            while (this._currentChar != Char.MinValue && Char.IsWhiteSpace(this._currentChar)) this.Advance();
        }

        private Token MakeNumber()
        {
            string result = String.Empty;

            while (this._currentChar != Char.MinValue && Char.IsDigit(this._currentChar))
            {
                result += this._currentChar;
                this.Advance();
            }

            if (this._currentChar == '.')
            {
                result += this._currentChar;
                this.Advance();

                while (this._currentChar != Char.MinValue && Char.IsDigit(this._currentChar))
                {
                    result += this._currentChar;
                    this.Advance();
                }

                return new Token(TokenType.Float, new FloatTokenValue((float)Double.Parse(result)), this._line, this._column);
            }

            return new Token(TokenType.Int, new IntTokenValue(Int32.Parse(result)), this._line, this._column);
        }

        private Token MakeId()
        {
            string result = String.Empty;

            while (this._currentChar != Char.MinValue && (Char.IsLetterOrDigit(this._currentChar) || this._currentChar == '_'))
            {
                result += this._currentChar;
                this.Advance();
            }

            return new Token(
                ReservedKeywords.ContainsKey(result) ? ReservedKeywords[result] : TokenType.Id,
                new StringTokenValue(result), this._line, this._column);
        }

        public Token GetNextToken()
        {
            while (this._currentChar != Char.MinValue)
            {
                if (Char.IsWhiteSpace(this._currentChar))
                {
                    this.SkipWhitespace();
                    continue;
                }
                else if (this._currentChar == '#')
                {
                    this.Advance();
                    this.SkipComment();
                    continue;
                }
                else if (Char.IsDigit(this._currentChar)) return this.MakeNumber();
                else if (Char.IsLetter(this._currentChar)) return this.MakeId();

                char currentChar = this._currentChar;
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
            throw new LexerError($"Unexpected character '{this._currentChar}' at line {this._line}, column {this._column}");
        }

        private void Log(string message)
        {
            if (Flags.LogLexer) Console.WriteLine($"Lexer: {message}");
        }
    }
}
