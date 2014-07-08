using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IEventSubscribersResolver
    {
        IEnumerable<string> GetSubscribersOf(Type eventType);
    }
}