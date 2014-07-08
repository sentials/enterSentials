using System;

namespace EnterSentials.Framework
{
    public class EventSourceLogAdapter : ILog
    {
        private readonly LogEventSource eventSource = null;


        public void CriticalError(string message)
        { eventSource.CriticalError(message); }

        public void Error(string message)
        { eventSource.Error(message); }

        public void Warning(string message)
        { eventSource.Warning(message); }

        public void Message(string message)
        { eventSource.Message(message); }

        public void ExceptionByPolicy(string message, Guid id)
        { eventSource.ExceptionByPolicy(message, id); }


        public EventSourceLogAdapter(LogEventSource eventSource)
        {
            Guard.AgainstNull(eventSource, "eventSource");
            this.eventSource = eventSource;
        }
    }
}
