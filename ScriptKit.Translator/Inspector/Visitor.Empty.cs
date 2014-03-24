﻿using ICSharpCode.NRefactory.CSharp;

namespace ScriptKit.NET
{
    public abstract partial class Visitor : IAstVisitor 
    {       
        public virtual void VisitCSharpTokenNode(CSharpTokenNode cSharpTokenNode)
        {
            //throw this.CreateException(cSharpTokenNode);
        }

        public virtual void VisitComment(Comment comment)
        {
            //throw this.CreateException(comment);
        }
        
        public virtual void VisitDefaultValueExpression(DefaultValueExpression defaultValueExpression)
        {            
            //throw this.CreateException(defaultValueExpression);
        }

        public virtual void VisitIdentifier(Identifier identifier)
        {
            //throw this.CreateException(identifier);
        }
        
        public virtual void VisitNullReferenceExpression(NullReferenceExpression nullReferenceExpression)
        {
            //throw this.CreateException(nullReferenceExpression);
        }
        
        public virtual void VisitPreProcessorDirective(PreProcessorDirective preProcessorDirective)
        {
            //throw this.CreateException(preProcessorDirective);
        }        

        public virtual void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
        {
            //throw this.CreateException(typeParameterDeclaration);
        }

        public virtual void VisitPrimitiveType(PrimitiveType primitiveType)
        {
            //throw this.CreateException(primitiveType);
        }

        public virtual void VisitSimpleType(SimpleType simpleType)
        {
            //throw this.CreateException(simpleType);
        }
    }
}
