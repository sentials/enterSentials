using Newtonsoft.Json;

namespace EnterSentials.Framework.Newtonsoft
{
    public interface INewtonsoftJsonSerializerFactory
    {
        JsonSerializer GetSerializer();
        JsonSerializerSettings GetSettings();
    }
}