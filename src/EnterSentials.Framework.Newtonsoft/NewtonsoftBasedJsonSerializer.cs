using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EnterSentials.Framework.Newtonsoft
{
    public class NewtonsoftBasedJsonSerializer : IJsonSerializer
    {
        private readonly INewtonsoftJsonSerializerFactory serializerFactory = null;
        private readonly JsonSerializerSettings settings = null;


        public string Serialize<T>(T @object)
        { return JsonConvert.SerializeObject(@object, settings); }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage", 
            "CA2202:Do not dispose objects multiple times",
            Justification="This technique is based on a reputable source on the internet: Carlos Figueira")]
        public byte[] SerializeToBytes<T>(T @object)
        {
            Guard.AgainstNull(@object, "object");

            var bytes = (byte[]) null;
            var serializer = serializerFactory.GetSerializer();

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
                {
                    using (var writer = new JsonTextWriter(streamWriter))
                    {
                        writer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, @object);
                        streamWriter.Flush();
                        bytes = stream.ToArray();
                    }
                }
            }

            return bytes;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage", 
            "CA2202:Do not dispose objects multiple times",
            Justification="This technique is based on a reputable source on the internet: Carlos Figueira")]
        public byte[] SerializeAndWrapToBytes(IEnumerable<SerializationParameter> parameters)
        {
            Guard.AgainstNull(parameters, "parameters");

            var bytes = (byte[])null;
            var serializer = serializerFactory.GetSerializer();

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
                {
                    using (var writer = new JsonTextWriter(streamWriter))
                    {
                        writer.Formatting = Formatting.Indented;

                        if (parameters.Count() == 1)
                            serializer.Serialize(streamWriter, parameters.First().Value);
                        else
                        {
                            writer.WriteStartObject();

                            foreach(var parameter in parameters.OrderBy(p => p.Index))
                            {
                                writer.WritePropertyName(parameter.Name);
                                serializer.Serialize(writer, parameter.Value);
                            }

                            writer.WriteEndObject();
                        }

                        writer.Flush();
                        streamWriter.Flush();
                        bytes = stream.ToArray();
                    }
                }
            } 

            return bytes;
        }


        public T Deserialize<T>(string @string)
        { return JsonConvert.DeserializeObject<T>(@string, settings); }
        
        public object Deserialize(string @string, Type targetType)
        { return JsonConvert.DeserializeObject(@string, targetType, settings); }

        public T Deserialize<T>(byte[] bytes)
        { return (T) Deserialize(bytes, typeof(T)); }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "This technique is based on a reputable source on the internet: Carlos Figueira")]
        public object Deserialize(byte[] bytes, Type targetType)
        {
            var serializer = serializerFactory.GetSerializer();

            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                { return serializer.Deserialize(streamReader, targetType); }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "This technique is based on a reputable source on the internet: Carlos Figueira")]
        public void DeserializeWrappedIntoParameters(byte[] bytes, IEnumerable<SerializationParameter> parametersMetadata, object[] parameters)
        {
            Guard.AgainstNull(bytes, "bytes");
            Guard.AgainstNull(parametersMetadata, "metadata");
            Guard.AgainstNull(parameters, "parameters");

            Guard.Against(parameters, p => parametersMetadata.Count() != p.Length, "Incompatible metadata and destination container.", "parameters");

            var metadata = parametersMetadata.ToKeyedCollection<SerializationParameterKeyedCollection, string, SerializationParameter>();

            var stream = new MemoryStream(bytes);
            var streamReader = new StreamReader(stream);
            var serializer = serializerFactory.GetSerializer();

            if (parameters.Count() == 1) // single parameter, assuming bare 
                parameters[0] = serializer.Deserialize(streamReader, metadata.First(m => m.Index == 0).Type);
            else
            {   // multiple parameters, needs to be wrapped
                var reader = new JsonTextReader(streamReader);
                reader.Read();
                Guard.Against(reader.TokenType != JsonToken.StartObject, "Input needs to be wrapped in an object");

                reader.Read();
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    var parameterName = reader.Value as string;
                    reader.Read();

                    if (metadata.Contains(parameterName))
                    {
                        var parameter = metadata[parameterName];
                        parameters[parameter.Index] = serializer.Deserialize(reader, parameter.Type);
                    }
                    else
                        reader.Skip();

                    reader.Read();
                }

                reader.Close();
            }

            streamReader.Close();
            stream.Close();
        }


        public NewtonsoftBasedJsonSerializer(INewtonsoftJsonSerializerFactory serializerFactory)
        {
            Guard.AgainstNull(serializerFactory, "serializerFactory");
            
            this.serializerFactory = serializerFactory;
            this.settings = serializerFactory.GetSettings();
            
            JsonConvert.DefaultSettings = () => serializerFactory.GetSettings();
        }
    }
}
