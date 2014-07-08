using System;
using System.Configuration;

namespace EnterSentials.Framework
{
    public abstract class TypeRepresentingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true, IsKey=true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}