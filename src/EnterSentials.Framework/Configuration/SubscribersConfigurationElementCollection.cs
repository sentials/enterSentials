using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class SubscribersConfigurationElementCollection : ConfigurationElementCollection<SubscriberConfigurationElement>
    {
        protected override string ElementName
        { get { return "subscriber"; } }


        public SubscribersConfigurationElementCollection()
        { }

        public SubscribersConfigurationElementCollection(SubscriberConfigurationElement[] subscribers) : base(subscribers)
        { }

        public SubscribersConfigurationElementCollection(IEnumerable<SubscriberConfigurationElement> subscribers) : base(subscribers)
        { }
    }
}
