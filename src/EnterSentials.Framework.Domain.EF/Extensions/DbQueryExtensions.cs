using EnterSentials.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace EnterSentials.Framework.Domain.EF
{
    public static class DbQueryExtensions
    {
        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static DbQuery<TResult> Include<TResult>(this DbQuery<TResult> query, IEnumerable<Expression<Func<TResult, object>>> propertyAccessingExpressions)
        {
            foreach (var propertyAccessingExpression in propertyAccessingExpressions)
                query = query.Include(propertyAccessingExpression.AsPropertyPath());
            return query;
        }


        // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/07/some-useful-entity-framework-extension.html
        public static DbQuery<TResult> Include<TResult>(this DbQuery<TResult> query, params Expression<Func<TResult, object>>[] propertyAccessingExpressions)
        { return query.Include((IEnumerable<Expression<Func<TResult, object>>>) propertyAccessingExpressions); }
    }
}
