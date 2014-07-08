using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public class ReadOnlyRepositoryWrapper<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> repository = null;


        public IQueryable<TEntity> AsQueryable()
        { return repository.AsQueryable(asReadOnly: true); }

        public TEntity Get(IEnumerable<object> keyValues, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.Get(keyValues, asReadOnly: true, eagerlyLoad: eagerlyLoad); }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> criteria, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.Where(criteria, orderBy: orderBy, asReadOnly: true, eagerlyLoad: eagerlyLoad); }

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.GetAll(orderBy: orderBy, asReadOnly: true, eagerlyLoad: eagerlyLoad); }

        public TEntity Single(Expression<Func<TEntity, bool>> criteria, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.Single(criteria: criteria, asReadOnly: true, eagerlyLoad: eagerlyLoad); }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> criteria, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.SingleOrDefault(criteria: criteria, asReadOnly: true, eagerlyLoad: eagerlyLoad); }

        public TEntity First(Expression<Func<TEntity, bool>> criteria, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.First(criteria: criteria, asReadOnly: true, eagerlyLoad: eagerlyLoad); }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> criteria, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return repository.FirstOrDefault(criteria: criteria, asReadOnly: true, eagerlyLoad: eagerlyLoad); }


        public ReadOnlyRepositoryWrapper(IRepository<TEntity> repository)
        {
            Guard.AgainstNull(repository, "repository");
            this.repository = repository;
        }
    }
}
