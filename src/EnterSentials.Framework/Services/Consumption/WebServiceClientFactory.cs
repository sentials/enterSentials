namespace EnterSentials.Framework
{
    public class WebServiceClientFactory : IWebServiceClientFactory
    {
        public static readonly WebServiceMessageFormat DefaultWebServiceMessageFormat = WebServiceMessageFormat.Json;

        public IWebServiceClient GetWebServiceClient()
        { return GetWebServiceClient(DefaultWebServiceMessageFormat); }

        public IWebServiceClient GetWebServiceClient(WebServiceMessageFormat format)
        {
            return format == WebServiceMessageFormat.Json
                ? new JsonWebServiceClient()
                : null;
        }
    }
}