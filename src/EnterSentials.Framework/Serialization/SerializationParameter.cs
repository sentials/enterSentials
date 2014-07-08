using System;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    public class SerializationParameter : SerializationProperty
    {
        public int Index { get; private set; }

        public SerializationParameter(int index, Type type, string name) : base(type, name)
        {
            Guard.AgainstOutOfRange(index, "index");
            Index = index;
        }
    }


    public class SerializationParameterKeyedCollection : KeyedCollection<string, SerializationParameter>
    {
        protected override string GetKeyForItem(SerializationParameter item)
        { return item.Name; }
    }
}