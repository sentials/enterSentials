using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class ExceptionShieldingWebHttpEndpointBehavior : WebHttpBehavior
    {
        private readonly IComponents components = null;


        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(components.Get<ExceptionManagerBasedErrorHandler>());
        }


        public ExceptionShieldingWebHttpEndpointBehavior(IComponents components)
        {
            Guard.AgainstNull(components, "components");
            this.components = components;
        }
    }
}