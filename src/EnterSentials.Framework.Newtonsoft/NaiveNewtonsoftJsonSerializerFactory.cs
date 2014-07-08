using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EnterSentials.Framework.Newtonsoft
{
    public class NaiveNewtonsoftJsonSerializerFactory : INewtonsoftJsonSerializerFactory
    {
        private IContractResolver GetContractResolver()
        { return new CamelCasePropertyNamesContractResolver(); }


        public JsonSerializer GetSerializer()
        { 
            var serializer = new JsonSerializer();
            serializer.ContractResolver = GetContractResolver();
            return serializer;
        }

        public JsonSerializerSettings GetSettings()
        { return new JsonSerializerSettings { ContractResolver = GetContractResolver() }; }
    }
}