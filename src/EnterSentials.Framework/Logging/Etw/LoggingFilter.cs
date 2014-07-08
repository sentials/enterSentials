using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public class LoggingFilter : ILoggingSinkTarget
    {
        public Type EventSourceType { get; private set; }
        public EventLevel EventLevel { get; private set; }
        public EventKeywords MatchAnyKeywords { get; private set; }
        public IDictionary<string, string> Arguments { get; private set; }

        public LoggingFilter(
            Type eventSourceType, 
            EventLevel eventLevel = EventLevel.Critical, 
            EventKeywords matcyAnyKeywords = EventKeywords.None, 
            IDictionary<string, string> arguments = null
        )
        {
            Guard.AgainstNull(eventSourceType, "eventSourceType");
            Guard.Against(eventSourceType, t => !t.IsSubclassOf<EventSource>(), "Must be an EventSource", "eventSourceType");

            EventSourceType = eventSourceType;
            EventLevel = eventLevel;
            MatchAnyKeywords = matcyAnyKeywords;
            Arguments = arguments;
        }
    }
}