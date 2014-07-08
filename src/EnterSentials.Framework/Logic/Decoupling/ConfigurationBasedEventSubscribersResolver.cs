using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public class ConfigurationBasedEventSubscribersResolver : IEventSubscribersResolver
    {
        private readonly IApplicationConfiguration configuration = null;


        public IEnumerable<string> GetSubscribersOf(Type eventType)
        {
            var @event = (EventConfigurationElement) null;
            var subscribers = Enumerable.Empty<string>();

            if ((configuration.Events != null) 
                && (configuration.Events.Events != null)
                && configuration.Events.Events.TryGet(eventType.GetConfigurationKey(), out @event))
            {
                if ((@event.Subscribers != null) && (@event.Subscribers.Subscribers != null))
                    subscribers = @event.Subscribers.Subscribers.Select(subscriber => subscriber.Key).ToArray();
            }
            
            return subscribers;
        }


        public ConfigurationBasedEventSubscribersResolver(IApplicationConfiguration configuration)
        {
            Guard.AgainstNull(configuration, "configuration");
            this.configuration = configuration;
        }
    }
}
