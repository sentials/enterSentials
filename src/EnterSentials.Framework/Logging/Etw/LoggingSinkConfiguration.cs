using System;

namespace EnterSentials.Framework
{
    public class LoggingSinkConfiguration
    {
        public Type Type { get; private set; }
        public object Parameters { get; private set; }

        public LoggingSinkConfiguration(Type sinkType, object sinkParameters = null)
        {
            Guard.AgainstNull(sinkType, "sinkType");
            Type = sinkType;
            Parameters = sinkParameters;
        }
    }
}