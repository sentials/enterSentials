using System;
using System.IO;
using System.Xml;

namespace EnterSentials.Framework
{
    public class DotNetBasedXmlSerializer : IXmlSerializer
    {
        private readonly IDotNetXmlSerializerFactory serializerFactory = null;


        public string Serialize<T>(T @object)
        {
            var serializer = serializerFactory.Get(typeof(T));
            var @string = (string) null;
            var writer = (StringWriter) null; ;
            using (var xmlWriter = XmlWriter.Create(writer = new StringWriter()))
            { 
                serializer.Serialize(xmlWriter, @object);
                @string = writer.ToString();
            }
            writer = null;
            return @string;
        }


        public T Deserialize<T>(string xml)
        { return (T) Deserialize(xml, typeof(T)); }

        public object Deserialize(string xml, Type targetType)
        {
            var @object = (object) null;
            var serializer = serializerFactory.Get(targetType);
            using (var reader = XmlReader.Create(new StringReader(xml)))
            { @object = serializer.Deserialize(reader); }
            return @object;
        }


        public DotNetBasedXmlSerializer(IDotNetXmlSerializerFactory serializerFactory)
        {
            Guard.AgainstNull(serializerFactory, "serializerFactory");
            this.serializerFactory = serializerFactory;
        }
    }
}