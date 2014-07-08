using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IContextProvidingEvent : IEvent
    {
        IEnumerable<Tuple<Type, object>> GetContext();
    }
}