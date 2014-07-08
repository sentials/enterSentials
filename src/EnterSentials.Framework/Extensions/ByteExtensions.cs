using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public static class ByteExtensions
    {
        public static T DeserializeJsonTo<T>(this byte[] bytes)
        { return Components.Instance.Get<IJsonSerializer>().Deserialize<T>(bytes); }

        public static object DeserializeJsonTo(this byte[] bytes, Type targetType)
        { return Components.Instance.Get<IJsonSerializer>().Deserialize(bytes, targetType); }


        public static void DeserializeWrappedIntoParameters(this byte[] bytes, IEnumerable<SerializationParameter> parametersMetadata, object[] parameters)
        { Components.Instance.Get<IJsonSerializer>().DeserializeWrappedIntoParameters(bytes, parametersMetadata, parameters); }
    }
}
