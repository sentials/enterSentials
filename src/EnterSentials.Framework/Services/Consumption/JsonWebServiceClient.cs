namespace EnterSentials.Framework
{
    public class JsonWebServiceClient : WebServiceClient
    {
        protected override string ContentType
        { get { return EnterSentials.Framework.ContentType.Json; } }

        protected override string SerializeData(object data)
        { return data.SerializeToJson(); }

        protected override TResponse Deserialize<TResponse>(string response)
        { return response.DeserializeJsonTo<TResponse>(); }
    }
}
