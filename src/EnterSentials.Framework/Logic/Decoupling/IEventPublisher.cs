using System.Collections.Generic;

namespace EnterSentials.Framework
{
    //public interface IEventPublisher
    //{
    //    void Publish(string eventName, IEnumerable<KeyValuePair<string, string>> properties);
    //}


    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event);
    }
}