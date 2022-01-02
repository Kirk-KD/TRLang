namespace TRLang.src.Parser.AbstractSyntaxTree
{
    class VarDecl : AstNode
    {
        public AstNode VarNode { get; private set; }
        public AstNode TypeNode { get; private set; }
        public AstNode ValueNode { get; private set; }

        public VarDecl(AstNode varNode, AstNode typeNode, AstNode valueNode)
        {
            this.VarNode = varNode;
            this.TypeNode = typeNode;
            this.ValueNode = valueNode;
        }

        public override string ToString() => $"VarDecl(VarNode={this.VarNode}, TypeNode={this.TypeNode}, ValueNode={this.ValueNode})";
    }
}
