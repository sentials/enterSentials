using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace EnterSentials.Framework.Domain.EF
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public interface IDomainContext : ICommittable, IDisposable
    {
        //ObjectSet<TEntity> GetContextFor<TEntity>() where TEntity : class, new();
        ObjectSet<TEntity> GetContextFor<TEntity>() where TEntity : class;
        //void SetStateOf<TEntity>(TEntity entity, EntityState state) where TEntity : class, new();
        void SetStateOf<TEntity>(TEntity entity, EntityState state) where TEntity : class;
        //EntityState GetStateOf<TEntity>(TEntity entity) where TEntity : class, new();
        EntityState GetStateOf<TEntity>(TEntity entity) where TEntity : class;
        //bool TryGetStateOf<TEntity>(TEntity entity, out EntityState state) where TEntity : class, new();
        bool TryGetStateOf<TEntity>(TEntity entity, out EntityState state) where TEntity : class;

        int ExecuteCommand(string commandText, params object[] parameters);

        ObjectResult<TElement> ExecuteQuery<TElement>(string queryText, params object[] parameters) where TElement : new();
        ObjectResult<TEntity> ExecuteQuery<TEntity>(string queryText, string entitySetName, bool asReadOnly, params object[] parameters) where TEntity : class, new();

        int ExecuteFunction(string functionName, params ObjectParameter[] parameters);
        ObjectResult<TElement> ExecuteFunction<TElement>(string functionName, params ObjectParameter[] parameters) where TElement : new();
        ObjectResult<TEntity> ExecuteFunction<TEntity>(string functionName, bool asReadOnly, params ObjectParameter[] parameters) where TEntity : class, new();

        int ExecuteStoredProcedure(string storedProcedureName, params ObjectParameter[] parameters);
        ObjectResult<TElement> ExecuteStoredProcedure<TElement>(string storedProcedureName, params ObjectParameter[] parameters) where TElement : new();
        ObjectResult<TEntity> ExecuteStoredProcedure<TEntity>(string storedProcedureName, bool asReadOnly, params ObjectParameter[] parameters) where TEntity : class, new();
    }
}
