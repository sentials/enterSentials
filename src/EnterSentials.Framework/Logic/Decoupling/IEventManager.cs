using System;

namespace EnterSentials.Framework
{
    public interface IEventManager : IDisposable
    {
        IDisposable Subscribe<TComponent>();
        IDisposable Subscribe(string componentKey);
    }


    public interface IEventManager<out TEvent> : IObservable<TEvent>, IEventManager
    { }
}