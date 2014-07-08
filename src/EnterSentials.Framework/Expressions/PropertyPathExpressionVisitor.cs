using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
    public class PropertyPathBuildingExpressionVisitor : ExpressionVisitor
    {
        public const string DefaultPropertyPathDelimiter = ".";

        private Stack<string> propertyNameStack = null;


        public Expression Expression
        { get; private set; }

        public string PropertyPath
        { get; private set; }


        private void BuildPropertyPath(string propertyPathDelimeter)
        {
            propertyNameStack = new Stack<string>();

            Visit(Expression);

            PropertyPath = propertyNameStack.Aggregate(
                new StringBuilder(),
                (sb, name) => (sb.Length > 0 ? sb.Append(propertyPathDelimeter) : sb).Append(name)
            ).ToString();

            propertyNameStack = null;
        }


        protected override Expression VisitMember(MemberExpression expression)
        {
            if (propertyNameStack != null)
                propertyNameStack.Push(expression.Member.Name);
            return base.VisitMember(expression);
        }


        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var visitedExpression = (Expression)expression;

            if (expression.Method.IsLinqOperator())
            {
                for (int i = 1; i < expression.Arguments.Count; i++)
                    Visit(expression.Arguments[i]);
                Visit(expression.Arguments[0]);
            }
            else
                visitedExpression = base.VisitMethodCall(expression);

            return visitedExpression;
        }


        public PropertyPathBuildingExpressionVisitor(Expression expression, string delimitedBy = null)
        {
            Expression = expression;
            BuildPropertyPath(delimitedBy ?? DefaultPropertyPathDelimiter);
        }
    }
}
