using System;
using System.Collections.Generic;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class Program : AstNode
    {
        public List<AstNode> Nodes { get; private set; }

        public Program(List<AstNode> nodes)
        {
            this.Nodes = nodes;
        }

        public override string ToString() => $"Program(Node={String.Join(", ", this.Nodes)})";
    }
}
