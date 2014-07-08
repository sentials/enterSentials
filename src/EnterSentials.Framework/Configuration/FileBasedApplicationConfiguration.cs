using EnterSentials.Framework.Properties;
using System;
using System.Configuration;

namespace EnterSentials.Framework
{
    public class FileBasedApplicationConfiguration : IApplicationConfiguration
    {
        private static readonly string configurationSectionName = Settings.Default.ApplicationConfigurationSectionName;
        private static readonly object configurationLock = new object();
        private static IApplicationConfiguration configuration = null;


        protected Configuration GetConfiguration(string filePath)
        { 
            var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = filePath };
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }


        protected virtual Configuration GetConfiguration()
        { 
            return ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap() { ExeConfigFilename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile },
                ConfigurationUserLevel.None
            );
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage", 
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification = "This is done safely and is the desired extensibility model."
        )]
        public FileBasedApplicationConfiguration()
        {
            if (configuration == null)
            {
                lock (configurationLock)
                {
                    if (configuration == null)
                        configuration = (IApplicationConfiguration) GetConfiguration().GetSection(configurationSectionName);
                }
            }
        }


        public PipelinesConfigurationElement Pipelines
        { get { return configuration == null ? null : configuration.Pipelines; } }

        public EventsConfigurationElement Events
        { get { return configuration == null ? null : configuration.Events; } }
    }
}
