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
}
