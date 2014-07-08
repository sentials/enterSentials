using System.Configuration;

namespace EnterSentials.Framework
{
    public class PipelinesConfigurationElement : ConfigurationElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PipelinesConfigurationElementCollection Pipelines
        {
            get { return (PipelinesConfigurationElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }
}
