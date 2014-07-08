using EnterSentials.Framework.Services.WCF.Properties;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace EnterSentials.Framework.Services.WCF
{
    public abstract class WebServiceHostFactoryBase : WebServiceHostFactory
    {
        private static readonly bool ShouldEnableCorsForAllWebServiceEndpoints = Settings.Default.ShouldEnableCorsForAllWebServiceEndpoints;
        

        protected abstract WebServiceHost NewServiceHost(Type serviceType, Uri[] baseAddresses);


        protected virtual void OnServiceHostCreated(WebServiceHost serviceHost)
        { }

        private void InternalOnServiceHostCreated(WebServiceHost serviceHost)
        {
            serviceHost.Opening -= InternalOnServiceHostOpening;
            serviceHost.Opening += InternalOnServiceHostOpening;

            OnServiceHostCreated(serviceHost);
        }


        protected virtual void OnServiceHostOpening(WebServiceHost serviceHost)
        { }

        private void InternalOnServiceHostOpening(object sender, EventArgs e)
        {
            var serviceHost = (WebServiceHost)sender;

            if (ShouldEnableCorsForAllWebServiceEndpoints)
            {
                foreach (var endpoint in serviceHost.Description.Endpoints.Where(se => se.Binding is WebHttpBinding))
                {
                    if (!endpoint.EndpointBehaviors.Any(b => b is EnableCorsAttribute))
                        endpoint.Behaviors.Add(new EnableCorsAttribute());

                    foreach (var operation in endpoint.Contract.Operations)
                        operation.OperationBehaviors.Add(new EnableCorsAttribute());
                }
            }

            OnServiceHostOpening(serviceHost);
        }


        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var serviceHost = NewServiceHost(serviceType, baseAddresses);
            InternalOnServiceHostCreated(serviceHost);
            return serviceHost;
        }
    }
}