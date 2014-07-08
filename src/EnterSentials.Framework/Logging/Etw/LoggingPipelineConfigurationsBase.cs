using System.Collections;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public abstract class LoggingPipelineConfigurationsBase : ILoggingPipelineConfigurations
    {
        protected abstract IEnumerable<LoggingPipelineConfiguration> GetConfigurations();


        public IEnumerator<LoggingPipelineConfiguration> GetEnumerator()
        { return GetConfigurations().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}