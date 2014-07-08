using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable(bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);

        TEntity Get(IEnumerable<object> keyValues, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity Create();
        TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity, new();
        TEntity Add(TEntity entity);
        TEntity Remove(IEnumerable<object> keyValues);
        TEntity Remove(TEntity entity);
        TEntity Update(TEntity entity);

        IEnumerable<TEntity> Where(
            Expression<Func<TEntity, bool>> criteria,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asReadOnly = false,
            IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null
        );

        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asReadOnly = false,
            IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null
        );


        TEntity Single(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity First(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null);
    }
}
