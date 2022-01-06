namespace TRLang.src.Lexer
{
    public enum TokenType
    {
        Int,
        Double,

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
        DoubleType,

        Id,
        Eof,

        Empty,

        // Reserved keywords
        Var,
        Func
    }
}
