using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class EventsConfigurationElementCollection : ConfigurationElementCollection<EventConfigurationElement>
    {
        protected override string ElementName
        { get { return "event"; } }


        public EventsConfigurationElementCollection()
        { }

        public EventsConfigurationElementCollection(EventConfigurationElement[] events) : base(events)
        { }

        public EventsConfigurationElementCollection(IEnumerable<EventConfigurationElement> events) : base(events)
        { }
    }
}