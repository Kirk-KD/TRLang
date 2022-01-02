using TRLang.src.Lexer;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Assign : AstNode
    {
        public AstNode LeftNode { get; private set; }
        public Token Op { get; private set; }
        public AstNode RightNode { get; private set; }

        public Assign(AstNode leftNode, Token op, AstNode rightNode)
        {
            this.LeftNode = leftNode;
            this.Op = op;
            this.RightNode = rightNode;
        }

        public override string ToString() => $"Assign(LeftNode={this.LeftNode}, Op={this.Op}, RightNode={this.RightNode}";
    }
}
