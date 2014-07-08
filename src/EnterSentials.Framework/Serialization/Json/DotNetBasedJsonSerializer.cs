using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class DotNetBasedJsonSerializer : IJsonSerializer
    {
        private readonly IDotNetJsonSerializerFactory serializerFactory = null;


        public string Serialize<T>(T @object)
        { return serializerFactory.Get().Serialize(@object); }

        public byte[] SerializeToBytes<T>(T @object)
        { throw new NotImplementedException(); }

        public byte[] SerializeAndWrapToBytes(IEnumerable<SerializationParameter> parameters)
        { throw new NotImplementedException(); }


        public T Deserialize<T>(string json)
        { return serializerFactory.Get().Deserialize<T>(json); }

        public object Deserialize(string json, Type targetType)
        { return serializerFactory.Get().Deserialize(json, targetType); }

        public T Deserialize<T>(byte[] bytes)
        { throw new NotImplementedException(); }
        
        public object Deserialize(byte[] bytes, Type targetType)
        { throw new NotImplementedException(); }

        public void DeserializeWrappedIntoParameters(byte[] bytes, IEnumerable<SerializationParameter> parametersMetadata, object[] parameters)
        { throw new NotImplementedException(); }


        public DotNetBasedJsonSerializer(IDotNetJsonSerializerFactory serializerFactory)
        {
            Guard.AgainstNull(serializerFactory, "serializerFactory");
            this.serializerFactory = serializerFactory;
        }
    }
}
