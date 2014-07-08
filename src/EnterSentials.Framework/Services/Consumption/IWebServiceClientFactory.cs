namespace EnterSentials.Framework
{
    public interface IWebServiceClientFactory
    {
        IWebServiceClient GetWebServiceClient();
        IWebServiceClient GetWebServiceClient(WebServiceMessageFormat format);
    }
}
