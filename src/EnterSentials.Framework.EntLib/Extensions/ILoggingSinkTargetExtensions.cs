using System.Linq;

namespace EnterSentials.Framework.EntLib
{
    public static class ILoggingSinkTargetExtensions
    {
        private static LoggingPipelineConfiguration Add(this ILoggingSinkTarget target, LoggingSinkConfiguration sinkConfiguration)
        {
            var configuration = target as LoggingPipelineConfiguration;

            var filter = target as LoggingFilter;
            if (filter != null)
                configuration = new LoggingPipelineConfiguration(filter);
            else
                Guard.Against(target, t => !(t is LoggingPipelineConfiguration), "Must be a configuration.", "target");

            return new LoggingPipelineConfiguration(
                configuration.Filter,
                configuration.Sinks.Concat(sinkConfiguration.ToEnumerable()).ToArray());
        }


        public static LoggingPipelineConfiguration LogToDebug(this ILoggingSinkTarget target)
        {
            Guard.AgainstNull(target, "target");
            return target.Add(new LoggingSinkConfiguration(typeof(DebugLoggingSink)));
        }


        public static LoggingPipelineConfiguration LogToConsole(this ILoggingSinkTarget target)
        {
            Guard.AgainstNull(target, "target");
            return target.Add(new LoggingSinkConfiguration(typeof(ConsoleLoggingSink)));
        }


        public static LoggingPipelineConfiguration LogToWindowsEventLog(
            this ILoggingSinkTarget target, 
            WindowsEventLogLoggingSink.Parameters parameters)
        {
            Guard.AgainstNull(target, "target");
            return target.Add(new LoggingSinkConfiguration(typeof(WindowsEventLogLoggingSink), parameters));
        }


        public static LoggingPipelineConfiguration LogToAzureTable(
            this ILoggingSinkTarget target,
            AzureTableLoggingSink.Parameters parameters)
        {
            Guard.AgainstNull(target, "target");
            return target.Add(new LoggingSinkConfiguration(typeof(AzureTableLoggingSink), parameters));
        }
    }
}
