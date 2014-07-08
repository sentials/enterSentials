using System;

namespace EnterSentials.Framework
{
    public interface IEventResolver
    {
        IEventManager<TEvent> Get<TEvent>();
        IEventManager Get(Type eventType);
    }
}