using System.Configuration;

namespace EnterSentials.Framework
{
    public class EventConfigurationElement : TypeRepresentingConfigurationElement
    {
        [ConfigurationProperty("subscribers", IsRequired = true)]
        public SubscribersConfigurationElement Subscribers
        {
            get { return (SubscribersConfigurationElement)base["subscribers"]; }
            set { base["subscribers"] = value; }
        }
    }
}