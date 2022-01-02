using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRLang.src.Lexer;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class UnaryOp : AstNode
    {
        public Token Op { get; private set; }
        public AstNode ExprNode { get; private set; }

        public UnaryOp(Token op, AstNode exprNode)
        {
            this.Op = op;
            this.ExprNode = exprNode;
        }

        public override string ToString() => $"UnaryOp(Op={this.Op}, ExprNode={this.ExprNode})";
    }
}
