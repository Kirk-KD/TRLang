using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    abstract class AstNode
    {
        public abstract override string ToString();

        public string GetTypeName() => this.GetType().ToString().Split('.').Last();
    }
}
