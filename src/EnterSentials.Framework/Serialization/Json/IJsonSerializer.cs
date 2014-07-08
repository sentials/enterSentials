using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IJsonSerializer : IStringSerializer
    {
        byte[] SerializeToBytes<T>(T @object);

        byte[] SerializeAndWrapToBytes(IEnumerable<SerializationParameter> parameters);

        T Deserialize<T>(byte[] bytes); 
        object Deserialize(byte[] bytes, Type targetType);
        void DeserializeWrappedIntoParameters(byte[] bytes, IEnumerable<SerializationParameter> parametersMetadata, object[] parameters);
    }
}