using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Program : AstNode
    {
        public List<AstNode> Nodes { get; private set; }

        public Program(List<AstNode> nodes)
        {
            this.Nodes = nodes;
        }

        public override string ToString() => $"Program(Node={String.Join(", ", this.Nodes)})";
    }
}
