using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public interface ILoggingPipelineFactory
    {
        LoggingPipeline Get(LoggingPipelineConfiguration pipelineConfiguration);
    }
}