using System;
using TRLang.src.Parser.AbstractSyntaxTree;
using Double = TRLang.src.Parser.AbstractSyntaxTree.Double;

namespace TRLang.src
{
    public abstract class AstNodeVisitor<T>
    {
        protected T GenericVisit(AstNode astNode)
        {
            this.BeforeVisit(astNode);

            switch (astNode)
            {
                case Int node: return this.Visit(node);
                case Double node: return this.Visit(node);
                case BinOp node: return this.Visit(node);
                case UnaryOp node: return this.Visit(node);
                case Compound node: return this.Visit(node);
                case NoOp node: return this.Visit(node);
                case Assign node: return this.Visit(node);
                case Var node: return this.Visit(node);
                case TypeSpec node: return this.Visit(node);
                case VarDecl node: return this.Visit(node);
                case FuncDecl node: return this.Visit(node);
                case FuncCall node: return this.Visit(node);
                case Program node: return this.Visit(node);

                default: throw new Exception($"No Visit method for AstNode {astNode.GetTypeName()}.");
            }
        }

        protected abstract void BeforeVisit(AstNode node);

        protected abstract T Visit(Int node);

        protected abstract T Visit(Double node);

        protected abstract T Visit(BinOp node);

        protected abstract T Visit(UnaryOp node);

        protected abstract T Visit(Compound node);

        protected abstract T Visit(NoOp node);

        protected abstract T Visit(Assign node);

        protected abstract T Visit(Var node);

        protected abstract T Visit(TypeSpec node);

        protected abstract T Visit(VarDecl node);

        protected abstract T Visit(FuncDecl node);

        protected abstract T Visit(FuncCall node);

        protected abstract T Visit(Program node);
    }
}
