using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnterSentials.Framework
{
    public class SimpleEventPublishingBuffer : IEventPublishingBuffer
    {
        private readonly ICollection<Action> eventPublishingActions = new Collection<Action>();


        protected IEventAggregator EventAggregator { get; private set; }
        

        public void Add<TEvent>(TEvent @event)
        { eventPublishingActions.Add(() => EventAggregator.Publish(@event)); }


        public void PublishAll()
        {
            var publishingActions = eventPublishingActions.ToArray();
            eventPublishingActions.Clear();
            publishingActions.ForEach(publish => publish());
        }


        public SimpleEventPublishingBuffer(IEventAggregator eventAggregator)
        {
            Guard.AgainstNull(eventAggregator, "eventAggregator");
            EventAggregator = eventAggregator;
        }
    }
}