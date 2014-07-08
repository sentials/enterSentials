using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public abstract class ContextProvidingEvent : Event, IContextProvidingEvent
    {
        protected virtual IEnumerable<object> GetContextObjects()
        { return null; }


        public IEnumerable<Tuple<Type, object>> GetContext()
        {
            var context = GetContextObjects();
            return (context != null
                ? context.Select(c => new Tuple<Type, object>(c.GetType(), c))
                : this.GetType().GetProperties().Select(property => new Tuple<Type, object>(property.PropertyType, property.GetValue(this)))
            ).ToArray();
        }
    }
}