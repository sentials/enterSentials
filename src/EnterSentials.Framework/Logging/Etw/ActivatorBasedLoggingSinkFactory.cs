using System;

namespace EnterSentials.Framework
{
    public class ActivatorBasedLoggingSinkFactory : ILoggingSinkFactory
    {
        public ILoggingSink Get(LoggingSinkConfiguration configuration)
        { 
            return (ILoggingSink) (configuration.Parameters == null
                ?  Activator.CreateInstance(configuration.Type)
                :  Activator.CreateInstance(configuration.Type, configuration.Parameters));
        }
    }
}