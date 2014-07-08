using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using UsingName = EnterSentials.Framework.Name;

namespace EnterSentials.Framework
{
    [EventSource(Name = Naming.ProductIdentifier)]
    public class LogEventSource : EventSource
    {
        private static readonly MethodInfo CriticalErrorMethod = typeof(LogEventSource).GetMethod(UsingName.Of.Method.On<LogEventSource>(l => l.CriticalError(null)));
        private static readonly MethodInfo ErrorMethod = typeof(LogEventSource).GetMethod(UsingName.Of.Method.On<LogEventSource>(l => l.Error(null)));
        private static readonly MethodInfo WarningMethod = typeof(LogEventSource).GetMethod(UsingName.Of.Method.On<LogEventSource>(l => l.Warning(null)));
        private static readonly MethodInfo MessageMethod = typeof(LogEventSource).GetMethod(UsingName.Of.Method.On<LogEventSource>(l => l.Message(null)));
        private static readonly MethodInfo ExceptionByPolicyMethod = typeof(LogEventSource).GetMethod(UsingName.Of.Method.On<LogEventSource>(l => l.ExceptionByPolicy(null, default(Guid))));


        [Event(LogEventId.CriticalError, Message = "{0}", Level = EventLevel.Critical, Task = LogTask.None)]
        public void CriticalError(string message)
        {
            var attribute = (EventAttribute)CriticalErrorMethod.GetCustomAttribute<EventAttribute>();
            if (IsEnabled(attribute.Level, attribute.Keywords))
                WriteEvent(attribute.EventId, message);
        }

        [Event(LogEventId.Error, Message = "{0}", Level = EventLevel.Error, Task = LogTask.None)]
        public void Error(string message)
        { 
            var attribute = (EventAttribute)ErrorMethod.GetCustomAttribute<EventAttribute>();
            if (IsEnabled(attribute.Level, attribute.Keywords))
                WriteEvent(attribute.EventId, message);
        }

        [Event(LogEventId.Warning, Message = "{0}", Level = EventLevel.Warning, Task = LogTask.None)]
        public void Warning(string message)
        { 
            var attribute = (EventAttribute)WarningMethod.GetCustomAttribute<EventAttribute>();
            if (IsEnabled(attribute.Level, attribute.Keywords))
                WriteEvent(attribute.EventId, message);
        }

        [Event(LogEventId.Message, Message = "{0}", Level = EventLevel.Informational, Task = LogTask.None)]
        public void Message(string message)
        { 
            var attribute = (EventAttribute)MessageMethod.GetCustomAttribute<EventAttribute>();
            if (IsEnabled(attribute.Level, attribute.Keywords))
                WriteEvent(attribute.EventId, message);
        }

        [Event(LogEventId.ExceptionByPolicy, Message = "{0}", Level = EventLevel.Error, Task = LogTask.None)]
        public void ExceptionByPolicy(string message, Guid id)
        { 
            var attribute = (EventAttribute)ExceptionByPolicyMethod.GetCustomAttribute<EventAttribute>();
            if (IsEnabled(attribute.Level, attribute.Keywords))
                WriteEvent(attribute.EventId, message, id);
        }
    }
}