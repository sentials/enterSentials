using System;

namespace EnterSentials.Framework
{
    public class BufferingEventAggregator : SimpleEventPublishingBuffer, IEventAggregator
    {
        public void Publish<TEvent>(TEvent @event)
        { Add(@event); }

        public IEventManager<TEvent> Get<TEvent>()
        { return EventAggregator.Get<TEvent>(); }

        public IEventManager Get(Type eventType)
        { return EventAggregator.Get(eventType); }


        public BufferingEventAggregator(IEventAggregator eventAggregator) : base(eventAggregator)
        { }
    }
}