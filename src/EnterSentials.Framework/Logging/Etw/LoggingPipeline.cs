using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public class LoggingPipeline : ScopeLimitedObject
    {
        public EventSource EventSource { get; private set; }
        public EventListener EventListener { get; private set; }
        public IEnumerable<ILoggingSink> Sinks { get; private set; }


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeImplicit();

            EventListener.DisableEvents(EventSource);

            Sinks.ForEach(s => s.Dispose());
            Sinks = null;

            EventListener.Dispose();
            EventListener = null;
            
            EventSource = null;
        }


        public LoggingPipeline(
            EventSource eventSource, 
            EventListener eventListener,
            IEnumerable<ILoggingSink> sinks)
        {
            Guard.AgainstNull(eventSource, "eventSource");
            Guard.AgainstNull(eventListener, "eventListener");
            Guard.AgainstNull(sinks, "sinks");

            EventSource = eventSource;
            EventListener = eventListener;
            Sinks = sinks;
        }
    }
}
