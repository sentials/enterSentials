using System;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    public static class ExpressionExtensions
    {
        public static bool IsMemberExpression<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression ignored = null;
            return expression.IsMemberExpression(out ignored);
        }

        public static bool IsMemberExpression<T>(this Expression<Func<T, object>> expression, out MemberExpression memberExpression)
        {
            memberExpression = null;

            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
                    break;
                default:
                    memberExpression = expression.Body as MemberExpression;
                    break;
            }

            return (memberExpression != null);
        }

        public static bool IsMethodCallExpression<T>(this Expression<Action<T>> expression)
        {
            MethodCallExpression ignored = null;
            return expression.IsMethodCallExpression(out ignored);
        }

        public static bool IsMethodCallExpression<T>(this Expression<Action<T>> expression, out MethodCallExpression methodCallExpression)
        {
            methodCallExpression = null;

            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    methodCallExpression = ((UnaryExpression)expression.Body).Operand as MethodCallExpression;
                    break;
                default:
                    methodCallExpression = expression.Body as MethodCallExpression;
                    break;
            }

            return (methodCallExpression != null);
        }

        public static MemberExpression ToMemberExpression<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression;
            if (!expression.IsMemberExpression(out memberExpression))
                throw new ArgumentException("The given expression is not a member-accessing expression", "expression");
            return memberExpression;
        }

        public static MethodCallExpression ToMethodCallExpression<T>(this Expression<Action<T>> expression)
        {
            MethodCallExpression methodCallExpression;
            if (!expression.IsMethodCallExpression(out methodCallExpression))
                throw new ArgumentException("The given expression is not a method-calling expression", "expression");
            return methodCallExpression;
        }


        public static string AccessedMemberName<T>(this Expression<Func<T, object>> memberAccessingExpression)
        { return memberAccessingExpression.ToMemberExpression().Member.Name; }

        public static string AccessedMemberName<T>(this Expression<Action<T>> methodCallExpression)
        { return methodCallExpression.ToMethodCallExpression().Method.Name; }


        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static string AsPropertyPath(this Expression expression, string delimitedBy = null)
        { return new PropertyPathBuildingExpressionVisitor(expression, delimitedBy).PropertyPath; }

        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static string AsConventionalHtmlInputName(this Expression expression)
        { return expression.AsPropertyPath(delimitedBy: "__"); }


        public static MemberExpression AsMemberExpression<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = null;
            expression.IsMemberExpression(out memberExpression);
            return memberExpression;
        }

        public static MethodCallExpression AsMethodCallExpression<T>(this Expression<Action<T>> expression)
        {
            MethodCallExpression methodCallExpression = null;
            expression.IsMethodCallExpression(out methodCallExpression);
            return methodCallExpression;
        }
    }
}
