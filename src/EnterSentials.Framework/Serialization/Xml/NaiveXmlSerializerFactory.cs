using System;
using System.Xml.Serialization;

namespace EnterSentials.Framework
{
    public class NaiveDotNetXmlSerializerFactory : IDotNetXmlSerializerFactory
    {
        public XmlSerializer Get(Type objectType)
        { return new XmlSerializer(objectType); }
    }
}