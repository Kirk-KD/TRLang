using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Param : AstNode
    {
        public AstNode VarNode { get; private set; }
        public AstNode TypeNode { get; private set; }

        public Param(AstNode varNode, AstNode typeNode)
        {
            this.VarNode = varNode;
            this.TypeNode = typeNode;
        }

        public override string ToString() => $"Param(VarNode={this.VarNode}, TypeNode={this.TypeNode})";
    }
}
