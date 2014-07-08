using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public class EmptyLoggingPipelineConfigurations : LoggingPipelineConfigurationsBase
    {
        protected override IEnumerable<LoggingPipelineConfiguration> GetConfigurations()
        { return Enumerable.Empty<LoggingPipelineConfiguration>(); }
    }
}