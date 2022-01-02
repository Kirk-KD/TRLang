using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class Empty : AstNode
    {
        public override string ToString() => "<Empty>";
    }
}
