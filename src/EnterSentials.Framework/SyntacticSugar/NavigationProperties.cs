using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    public static class Navigation
    {
        public static class Properties
        {
            public static IEnumerable<Expression<Func<T, object>>> Of<T>(params Expression<Func<T, object>>[] propertyAccessingExpressions)
            { return propertyAccessingExpressions; }
        }
    }
}
