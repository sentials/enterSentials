using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable();

        TEntity Get(IEnumerable<object> keyValues, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);

        IEnumerable<TEntity> Where(
            Expression<Func<TEntity, bool>> criteria,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null
        );

        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null
        );


        TEntity Single(Expression<Func<TEntity, bool>> criteria = null, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> criteria = null, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity First(Expression<Func<TEntity, bool>> criteria = null, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> criteria = null, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
    }
}
