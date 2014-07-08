using System;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    public class SerializationProperty
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public object Value { get; set; }

        public SerializationProperty(Type type, string name)
        {
            Guard.AgainstNull(type, "type");
            Guard.AgainstNullOrEmpty(name, "name");
            Type = type;
            Name = name;
        }
    }


    public class SerializationPropertyKeyedCollection : KeyedCollection<string, SerializationProperty>
    {
        protected override string GetKeyForItem(SerializationProperty item)
        { return item.Name; }
    }
}