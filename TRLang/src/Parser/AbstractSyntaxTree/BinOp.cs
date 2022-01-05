using TRLang.src.Lexer;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class BinOp : AstNode
    {
        public AstNode LeftNode { get; private set; }
        public Token Op { get; private set; }
        public AstNode RightNode { get; private set; }

        public BinOp(AstNode left, Token op, AstNode right)
        {
            this.LeftNode = left;
            this.Op = op;
            this.RightNode = right;
        }

        public override string ToString() => $"BinOp(LeftNode={this.LeftNode}, Op={this.Op}, RightNode={this.RightNode})";
    }
}
