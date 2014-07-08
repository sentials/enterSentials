using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public class ComponentsBasedLoggingPipelineFactory : ILoggingPipelineFactory
    {
        private readonly IComponents components = null;
        private readonly ILoggingSinkFactory loggingSinkFactory = null;


        public LoggingPipeline Get(LoggingPipelineConfiguration pipelineConfiguration)
        {
            var filter = pipelineConfiguration.Filter;

            var eventSource = (EventSource)components.Get(filter.EventSourceType);
            Guard.Against(eventSource == null, string.Format("Must be able to get event source of type: {0}", filter.EventSourceType.Name));

            var listener = components.Get<EventListener>();
            listener.EnableEvents(eventSource, filter.EventLevel, filter.MatchAnyKeywords, filter.Arguments);

            var sinks = new Collection<ILoggingSink>();
            foreach (var sinkConfiguration in pipelineConfiguration.Sinks)
            {
                var sink = loggingSinkFactory.Get(sinkConfiguration);
                sink.AttachTo(listener);
                sinks.Add(sink);
            }

            return new LoggingPipeline(eventSource, listener, sinks);
        }


        public ComponentsBasedLoggingPipelineFactory(IComponents components, ILoggingSinkFactory loggingSinkFactory)
        {
            Guard.AgainstNull(components, "components");
            Guard.AgainstNull(loggingSinkFactory, "loggingSinkFactory");
            this.components = components;
            this.loggingSinkFactory = loggingSinkFactory;
        }
    }
}
