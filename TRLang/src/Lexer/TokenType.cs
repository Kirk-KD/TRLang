namespace TRLang.src.Lexer
{
    public enum TokenType
    {
        Int,
        Double,
        String,

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
        StringType,

        Id,
        Eof,

        Empty,

        // Reserved keywords
        Var,
        Func
    }
}
