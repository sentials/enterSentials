using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class ComponentsBasedEventAggregator : IEventAggregator
    {
        private readonly IDictionary<Type, IEventManager> subjects = new Dictionary<Type, IEventManager>();
        private readonly IComponents components = null;


        public IEventManager<TEvent> Get<TEvent>()
        { return (IEventManager<TEvent>)Get(typeof(TEvent)); }


        public IEventManager Get(Type eventType)
        {
            var subject = (IEventManager)null;
            if (!subjects.TryGetValue(eventType, out subject))
                subject = subjects[eventType] = (IEventManager) components.Get(typeof(EventSubject<>).MakeGenericType(eventType));
            return subject;
        }


        public void Publish<TEvent>(TEvent @event)
        {
            var subject = (IEventManager) null;
            if (subjects.TryGetValue(typeof(TEvent), out subject))
                ((EventSubject<TEvent>)subject).OnNext(@event);
        }


        public ComponentsBasedEventAggregator(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }
    }
}