using System.Configuration;

namespace EnterSentials.Framework
{
    public class EventsConfigurationElement : ConfigurationElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public EventsConfigurationElementCollection Events
        {
            get { return (EventsConfigurationElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }
}
