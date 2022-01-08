using System;
using System.Collections.Generic;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public class Compound : AstNode
    {
        public List<AstNode> Children { get; private set; }

        public Compound()
        {
            this.Children = new List<AstNode>();
        }

        public override string ToString() => $"Compound(Children=[{string.Join(", ", this.Children)})])";
    }
}
