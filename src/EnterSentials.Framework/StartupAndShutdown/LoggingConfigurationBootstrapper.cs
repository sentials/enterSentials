namespace EnterSentials.Framework
{
    public class LoggingConfigurationBootstrapper : IBootstrapper
    {
        public void Initialize()
        { 
            // Only thing to do is resolve the pipeelins which will be configured upon resolution. 
        }
        

        public LoggingConfigurationBootstrapper(ILoggingPipelines loggingPipelines)
        {
            // Only thing to do is resolve the pipeelins which will be configured upon resolution.
            var pipelines = loggingPipelines;
        }
    }
}