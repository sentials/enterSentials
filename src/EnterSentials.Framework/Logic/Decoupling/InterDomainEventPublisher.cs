using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterSentials.Framework
{
    public class InterDomainEventPublisher : IEventPublisher
    {
        public void Publish(string eventName, IEnumerable<KeyValuePair<string, string>> properties)
        {
            
        }

        //private class PublishableEvent
        //{
        //    public string Name { get; private set; }
        //    public IEnumerable<KeyValuePair<string, string>> Properties { get; private set; }

        //    private IEnumerable<KeyValuePair<string, string>> GetPropertiesFrom(object payload)
        //    {
        //        return payload.GetEnumeratedProperties()
        //            .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString()))
        //            .ToArray();
        //    }

        //    public PublishableEvent(string name, object payload)
        //    {
        //        Guard.AgainstNullOrEmpty(name, "name");
        //        Guard.AgainstNull(payload, "payload");
        //        Name = name;
        //        Properties = GetPropertiesFrom(payload);
        //    }
        //}
    }
}
