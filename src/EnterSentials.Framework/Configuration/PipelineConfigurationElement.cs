using System;
using System.Configuration;

namespace EnterSentials.Framework
{
    public class PipelineConfigurationElement : KeyedByNameOrTypeConfigurationElement
    {
        [ConfigurationProperty("filters", IsRequired = true)]
        public FiltersConfigurationElement Filters
        {
            get { return (FiltersConfigurationElement)base["filters"]; }
            set { base["filters"] = value; }
        }
    }
}