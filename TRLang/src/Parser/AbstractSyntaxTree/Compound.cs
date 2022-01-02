using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Compound : AstNode
    {
        public List<AstNode> Children { get; private set; }

        public Compound()
        {
            this.Children = new List<AstNode>();
        }

        public override string ToString() => $"Compound(Children=[{String.Join(", ", this.Children)})])";
    }
}
