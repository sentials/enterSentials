using System.Configuration;

namespace EnterSentials.Framework
{
    public class FiltersConfigurationElement : ConfigurationElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public FiltersConfigurationElementCollection Filters
        {
            get { return (FiltersConfigurationElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }
}