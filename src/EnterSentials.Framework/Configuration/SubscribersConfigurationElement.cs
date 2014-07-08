using System.Configuration;

namespace EnterSentials.Framework
{
    public class SubscribersConfigurationElement : ConfigurationElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SubscribersConfigurationElementCollection Subscribers
        {
            get { return (SubscribersConfigurationElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }
}