using System.Configuration;

namespace EnterSentials.Framework
{
    public class KeyedByNameOrTypeConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)base["key"]; }
            set { base["key"] = value; }
        }
    }
}