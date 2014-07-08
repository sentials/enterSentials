using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public class LoggingPipelineConfiguration : ILoggingSinkTarget
    {
        public LoggingFilter Filter { get; private set; }
        public IEnumerable<LoggingSinkConfiguration> Sinks { get; private set; }


        public LoggingPipelineConfiguration(
            LoggingFilter filter,
            IEnumerable<LoggingSinkConfiguration> sinks
        )
        {
            Guard.AgainstNull(filter, "filter");
            Filter = filter;
            Sinks = sinks ?? Enumerable.Empty<LoggingSinkConfiguration>();
        }


        public LoggingPipelineConfiguration(
            LoggingFilter filter,
            params LoggingSinkConfiguration[] sinks
        ) : this(filter, sinks ?? Enumerable.Empty<LoggingSinkConfiguration>())
        { }
    }
}