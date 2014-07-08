using System.Configuration;

namespace EnterSentials.Framework
{
    public class ApplicationConfigurationSection : ConfigurationSection, IApplicationConfiguration
    {
        [ConfigurationProperty("pipelines")]
        public PipelinesConfigurationElement Pipelines
        {
            get { return (PipelinesConfigurationElement)base["pipelines"]; }
            set { base["pipelines"] = value; }
        }


        [ConfigurationProperty("events")]
        public EventsConfigurationElement Events
        {
            get { return (EventsConfigurationElement)base["events"]; }
            set { base["events"] = value; }
        }
    }
}