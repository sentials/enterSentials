using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EnterSentials.Framework.Domain.EF
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public class DomainContextBasedRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected static readonly object EntityMetadataLock = new object();
        protected static bool EntityMetadataInitialized = false;

        protected static IEnumerable<PropertyInfo> KeyProperties = null;


        protected IDomainContext DomainContext
        { get; private set; }

        protected IDomainLogicDispatcher DomainLogicDispatcher
        { get; private set; }

        
        protected Expression<Func<TEntity, bool>> GetKeyComparisonExpression(IEnumerable<object> keyValues)
        {
            var propertyEqualityExpressions = new Queue<Expression>();

            var keyPropertiesEnumerator = KeyProperties.GetEnumerator();
            var keyValuesEnumerator = keyValues.GetEnumerator();

            var entityParam = Expression.Parameter(typeof(TEntity), "entity");

            while (keyPropertiesEnumerator.MoveNext() && keyValuesEnumerator.MoveNext())
                propertyEqualityExpressions.Enqueue(
                    Expression.Equal(
                        Expression.Property(entityParam, keyPropertiesEnumerator.Current),
                        Expression.Constant(keyValuesEnumerator.Current)
                    )
                );

            var expression = propertyEqualityExpressions.Dequeue();
            while (propertyEqualityExpressions.Any())
                expression = Expression.AndAlso(expression, propertyEqualityExpressions.Dequeue());

            return Expression.Lambda<Func<TEntity, bool>>(expression, entityParam);
        }



        protected virtual ObjectSet<TEntity> GetObjectSet(bool asReadOnly = false)
        {
            var objectSet = DomainContext.GetContextFor<TEntity>();
            if (asReadOnly)
                objectSet.MergeOption = MergeOption.NoTracking;
            return objectSet;
        }

        protected virtual ObjectQuery<TEntity> GetQuery(bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        {
            var query = (ObjectQuery<TEntity>)GetObjectSet(asReadOnly);
            if (eagerlyLoad != null)
                foreach (var propertyToEagerLoad in eagerlyLoad)
                    query = query.Include(propertyToEagerLoad.AsPropertyPath());
            return query;
        }


        protected ObjectQuery<TEntity> GetQuery(Expression<Func<TEntity, bool>> criteria, bool asReadOnly, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad)
        {
            var query = GetQuery(asReadOnly, eagerlyLoad);
            return (ObjectQuery<TEntity>)(criteria != null ? query.Where(criteria) : query);
        }


        public virtual IQueryable<TEntity> AsQueryable(bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return DomainLogicDispatcher.Dispatch(() => GetQuery(asReadOnly, eagerlyLoad)); }


        protected virtual TEntity Get(ObjectQuery<TEntity> objectSet, IEnumerable<object> keyValues)
        {
            Guard.AgainstNull(keyValues, "keyValues");
            Guard.Against(keyValues, kv => !kv.Any(), "A key must be provided.", "keyValues");
            Guard.Against(keyValues, kv => kv.Count() != KeyProperties.Count(), "Mismatch in number of key properties and key values provided.", "keyValues");
            return objectSet.Where(GetKeyComparisonExpression(keyValues)).FirstOrDefault();
        }


        public TEntity Get(IEnumerable<object> keyValues, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        {
            Guard.AgainstNull(keyValues, "keyValues");
            Guard.Against(keyValues, kv => !kv.Any(), "A key must be provided.", "keyValues");
            Guard.Against(keyValues, kv => kv.Count() != KeyProperties.Count(), "Mismatch in number of key properties and key values provided.", "keyValues");
            return DomainLogicDispatcher.Dispatch(() => Where(GetKeyComparisonExpression(keyValues), asReadOnly: asReadOnly, eagerlyLoad: eagerlyLoad).FirstOrDefault());
        }

        public virtual TEntity Create()
        { return DomainLogicDispatcher.Dispatch(() => GetObjectSet().CreateObject()); }

        public virtual TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity, new()
        { return DomainLogicDispatcher.Dispatch(() => GetObjectSet().CreateObject<TDerivedEntity>()); }

        public virtual TEntity Add(TEntity entity)
        {
            return DomainLogicDispatcher.Dispatch(() =>
            {
                Guard.AgainstNull(entity, "entity");

                // Applying conventions here just in case non-EF-based repository is in use or a custom SqlGenerator isn't in use for an EF-based one
                var creationDatePersistingEntity = (entity as ICreationDatePersisting);
                if (creationDatePersistingEntity != null)
                    creationDatePersistingEntity.CreatedOn = DomainPolicy.GetCurrentTime();

                var lastModifiedDatePersistingEntity = (entity as ILastModifiedDatePersisting);
                if (lastModifiedDatePersistingEntity != null)
                    lastModifiedDatePersistingEntity.LastModifiedOn = DomainPolicy.GetCurrentTime();

                var auditableEntity = (entity as IAuditable);
                if (auditableEntity != null)
                    auditableEntity.IsActive = DomainPolicy.AuditableDomainObjectIsActiveOnCreation;

                GetObjectSet().AddObject(entity);
                return entity;
            });
        }

        public virtual TEntity Remove(IEnumerable<object> keyValues)
        {
            return DomainLogicDispatcher.Dispatch(() =>
            {
                var objectSet = GetObjectSet();
                var entity = Get(objectSet, keyValues);
                Guard.Against(entity == null, "No entity exists with the provided key.");
                objectSet.DeleteObject(entity);
                return entity;
            });
        }

        public virtual TEntity Remove(TEntity entity)
        {
            return DomainLogicDispatcher.Dispatch(() =>
            {
                var objectSet = GetObjectSet();
                if (DomainContext.GetStateOf(entity) == EntityState.Detached)
                    objectSet.Attach(entity);
                objectSet.DeleteObject(entity);
                return entity;
            });
        }

        public virtual TEntity Update(TEntity entity)
        {
            return DomainLogicDispatcher.Dispatch(() =>
            {
                try
                {
                    var lastModifiedDatePersistingEntity = (entity as ILastModifiedDatePersisting);
                    if (lastModifiedDatePersistingEntity != null)
                        lastModifiedDatePersistingEntity.LastModifiedOn = DomainPolicy.GetCurrentTime();

                    GetObjectSet().Attach(entity);
                    DomainContext.SetStateOf(entity, EntityState.Modified);
                }
                catch (InvalidOperationException)
                { entity = GetObjectSet().ApplyCurrentValues(entity); }

                return entity;
            });
        }


        public virtual IEnumerable<TEntity> Where(
            Expression<Func<TEntity, bool>> criteria,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asReadOnly = false,
            IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null
        )
        {
            return DomainLogicDispatcher.Dispatch(() =>
            {
                var query = GetQuery(criteria, asReadOnly, eagerlyLoad);
                return ((orderBy != null) ? orderBy(query) : query).ToArray();
            });
        }


        public virtual IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool asReadOnly = false,
            IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null
        )
        { return DomainLogicDispatcher.Dispatch(() => Where(entity => true, orderBy, asReadOnly, eagerlyLoad)); }


        public virtual TEntity Single(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return DomainLogicDispatcher.Dispatch(() => GetQuery(criteria, asReadOnly, eagerlyLoad).Single(criteria)); }


        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return DomainLogicDispatcher.Dispatch(() => GetQuery(criteria, asReadOnly, eagerlyLoad).SingleOrDefault(criteria)); }


        public virtual TEntity First(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return DomainLogicDispatcher.Dispatch(() => GetQuery(criteria, asReadOnly, eagerlyLoad).First(criteria)); }


        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> criteria = null, bool asReadOnly = false, IEnumerable<Expression<Func<TEntity, object>>> eagerlyLoad = null)
        { return DomainLogicDispatcher.Dispatch(() => GetQuery(criteria, asReadOnly, eagerlyLoad).FirstOrDefault(criteria)); }



        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage", 
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification="The functionality of this component requires this for optimization."
        )]
        public DomainContextBasedRepository(IDomainContext domainContext, IDomainLogicDispatcher domainLogicDispatcher)
        {
            Guard.AgainstNull(domainContext, "domainContext");
            Guard.AgainstNull(domainLogicDispatcher, "domainLogicDispatcher");

            this.DomainContext = domainContext;
            this.DomainLogicDispatcher = domainLogicDispatcher;

            if (!EntityMetadataInitialized)
            {
                lock (EntityMetadataLock)
                {
                    if (!EntityMetadataInitialized)
                    {
                        var defaultKeyOrder = 0;
                        var keyProperties = GetObjectSet().EntitySet.ElementType.KeyMembers
                            .Select(m => typeof(TEntity).GetProperty(m.Name))
                            .Select(property =>
                            {
                                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                                return new Tuple<int, PropertyInfo>((columnAttribute == null) ? defaultKeyOrder : columnAttribute.Order, property);
                            });


                        KeyProperties = keyProperties.OrderBy(tuple => tuple.Item1).Select(tuple => tuple.Item2);
                        EntityMetadataInitialized = true;
                    }
                }
            }
        }
    }
}
