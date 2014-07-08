using System;
using System.Xml.Serialization;

namespace EnterSentials.Framework
{
    public interface IDotNetXmlSerializerFactory
    {
        XmlSerializer Get(Type objectType);
    }
}