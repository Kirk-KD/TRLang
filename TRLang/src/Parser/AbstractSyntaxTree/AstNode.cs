using System.Linq;

namespace TRLang.src.Parser.AbstractSyntaxTree
{
    public abstract class AstNode
    {
        public abstract override string ToString();

        public string GetTypeName() => this.GetType().ToString().Split('.').Last();
    }
}
