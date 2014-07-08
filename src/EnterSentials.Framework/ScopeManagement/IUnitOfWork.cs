using System;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public interface IUnitOfWork : ICommittable, IDisposable
    {
        IUnitOfWorkFactory Factory { get; }
        IQueryFactory Queries { get; }
        ICommandFactory Commands { get; }
        IEventAggregator Events { get; }
        IExceptionManager ExceptionManager { get; }

        bool TryAdd(Type componentType, object component);
        bool TryAdd<TComponent>(TComponent component);
        
        IUnitOfWork Add(Type componentType, object component);
        IUnitOfWork Add<TComponent>(TComponent component);

        bool TryGet(Type componentType, out object component);
        bool TryGet<TComponent>(out TComponent component); 
        
        object Get(Type type);
        TComponent Get<TComponent>();
    }
}