using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace EnterSentials.Framework.Domain.EF
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public abstract class ObjectContextBasedDomainContext : ScopeLimitedObject, IDomainContext
    {
        protected abstract ObjectContext ObjectContext { get; }


        //public virtual ObjectSet<TEntity> GetContextFor<TEntity>() where TEntity : class, new()
        public virtual ObjectSet<TEntity> GetContextFor<TEntity>() where TEntity : class
        { return ObjectContext.CreateObjectSet<TEntity>(); }

        //public virtual void SetStateOf<TEntity>(TEntity entity, EntityState state) where TEntity : class, new()
        public virtual void SetStateOf<TEntity>(TEntity entity, EntityState state) where TEntity : class
        { ObjectContext.ObjectStateManager.ChangeObjectState(entity, state); }

        //public virtual EntityState GetStateOf<TEntity>(TEntity entity) where TEntity : class, new()
        public virtual EntityState GetStateOf<TEntity>(TEntity entity) where TEntity : class
        { return ObjectContext.ObjectStateManager.GetObjectStateEntry(entity).State; }

        //public bool TryGetStateOf<TEntity>(TEntity entity, out EntityState entityState) where TEntity : class, new()
        public bool TryGetStateOf<TEntity>(TEntity entity, out EntityState entityState) where TEntity : class
        {
            var objectStateEntry = (ObjectStateEntry)null;
            var canOrNot = ObjectContext.ObjectStateManager.TryGetObjectStateEntry(entity, out objectStateEntry);
            entityState = canOrNot ? objectStateEntry.State : default(EntityState);
            return canOrNot;
        }


        public int ExecuteCommand(string commandText, params object[] parameters)
        { return ObjectContext.ExecuteStoreCommand(commandText, parameters); }


        public ObjectResult<TElement> ExecuteQuery<TElement>(string queryText, params object[] parameters) where TElement : new()
        { return ObjectContext.ExecuteStoreQuery<TElement>(queryText, parameters); }

        public ObjectResult<TEntity> ExecuteQuery<TEntity>(string queryText, string entitySetName, bool asReadOnly, params object[] parameters) where TEntity : class, new()
        { return ObjectContext.ExecuteStoreQuery<TEntity>(queryText, entitySetName, asReadOnly ? MergeOption.NoTracking : MergeOption.AppendOnly, parameters); }


        public int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        { return ObjectContext.ExecuteFunction(functionName, parameters); }

        public ObjectResult<TElement> ExecuteFunction<TElement>(string functionName, params ObjectParameter[] parameters) where TElement : new()
        { return ObjectContext.ExecuteFunction<TElement>(functionName, parameters); }

        public ObjectResult<TEntity> ExecuteFunction<TEntity>(string functionName, bool asReadOnly, params ObjectParameter[] parameters) where TEntity : class, new()
        { return ObjectContext.ExecuteFunction<TEntity>(functionName, asReadOnly ? MergeOption.NoTracking : MergeOption.AppendOnly, parameters); }


        public int ExecuteStoredProcedure(string storedProcedureName, params ObjectParameter[] parameters)
        { return ObjectContext.ExecuteFunction(storedProcedureName, parameters); }

        public ObjectResult<TElement> ExecuteStoredProcedure<TElement>(string storedProcedureName, params ObjectParameter[] parameters) where TElement : new()
        { return ObjectContext.ExecuteFunction<TElement>(storedProcedureName, parameters); }

        public ObjectResult<TEntity> ExecuteStoredProcedure<TEntity>(string storedProcedureName, bool asReadOnly, params ObjectParameter[] parameters) where TEntity : class, new()
        { return ObjectContext.ExecuteFunction<TEntity>(storedProcedureName, asReadOnly ? MergeOption.NoTracking : MergeOption.AppendOnly, parameters); }


        public virtual void Commit()
        { ObjectContext.SaveChanges(); }
    }
}
