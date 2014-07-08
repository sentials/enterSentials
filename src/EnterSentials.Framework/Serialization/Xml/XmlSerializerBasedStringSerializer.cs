using System;

namespace EnterSentials.Framework
{
    public class XmlSerializerBasedStringSerializer : IStringSerializer
    {
        private readonly IXmlSerializer serializer = null;


        public string Serialize<T>(T @object)
        { return serializer.Serialize<T>(@object); }


        public object Deserialize(string @string, Type targetType)
        { return serializer.Deserialize(@string, targetType); }

        public T Deserialize<T>(string @string)
        { return serializer.Deserialize<T>(@string); }


        public XmlSerializerBasedStringSerializer(IXmlSerializer serializer)
        {
            Guard.AgainstNull(serializer, "serializer");
            this.serializer = serializer;
        }
    }
}