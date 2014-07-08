using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public static class SerializationParameterExtensions
    {
        public static byte[] SerializeAndWrapToBytes(this IEnumerable<SerializationParameter> parametersMetadata)
        { return Components.Instance.Get<IJsonSerializer>().SerializeAndWrapToBytes(parametersMetadata); }


        public static SerializationParameterKeyedCollection Clone(this SerializationParameterKeyedCollection parameters)
        {
            var collection = new SerializationParameterKeyedCollection();
            foreach (var parameter in parameters)
                collection.Add(parameter.Clone());
            return collection;
        }
    }
}