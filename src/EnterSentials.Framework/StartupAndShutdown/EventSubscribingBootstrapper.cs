using System;
using System.Linq;

namespace EnterSentials.Framework
{
    public class EventSubscribingBootstrapper : IBootstrapper
    {
        private readonly IEventSubscribersResolver eventSubscribersResolver = null;
        private readonly IEventAggregator eventAggregator = null;

        public void Initialize()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetProductAssemblies())
                foreach (var eventType in assembly.GetTypes().Where(t => t.Implements<IEvent>()))
                    foreach (var subscriber in eventSubscribersResolver.GetSubscribersOf(eventType))
                        eventAggregator.Get(eventType).Subscribe(subscriber);
        }

        public EventSubscribingBootstrapper(
            IEventSubscribersResolver eventSubscribersResolver,
            IEventAggregator eventAggregator
        )
        {
            Guard.AgainstNull(eventSubscribersResolver, "eventSubscribersResolver");
            Guard.AgainstNull(eventAggregator, "eventAggregator");

            this.eventSubscribersResolver = eventSubscribersResolver;
            this.eventAggregator = eventAggregator;
        }
    }
}
