using EnterSentials.Framework.EntLib.Properties;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework.EntLib
{
    public class DefaultLoggingPipelineConfigurations : LoggingPipelineConfigurationsBase, ILoggingPipelineConfigurations
    {
        private static readonly EventLevel LoggingEventLevel = Settings.Default.DefaultLoggingEventLevel;

        private static readonly LoggingFilter DefaultFilter = new LoggingFilter(typeof(LogEventSource), LoggingEventLevel);


        protected override IEnumerable<LoggingPipelineConfiguration> GetConfigurations()
        {
            return new LoggingPipelineConfiguration[]
            {
                DefaultFilter
                    .LogToDebug()
                    .LogToWindowsEventLog(new WindowsEventLogLoggingSink.Parameters())
                    .LogToAzureTable(new AzureTableLoggingSink.Parameters()),
            };
        }
    }
}