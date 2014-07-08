using System;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    public static class Name
    {
        public static class Of
        {
            public static class Property
            {
                public static string On<T>(Expression<Func<T, object>> propertyAccessorExpression)
                { return propertyAccessorExpression.AccessedMemberName(); }
            }

            public static class Method
            {
                public static string On<T>(Expression<Action<T>> methodCallExpression)
                { return methodCallExpression.AccessedMemberName(); }
            }
        }
    }
}
