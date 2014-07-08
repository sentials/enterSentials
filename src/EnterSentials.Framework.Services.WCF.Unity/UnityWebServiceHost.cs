using Microsoft.Practices.Unity;
using System;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    public class UnityWebServiceHost : WebServiceHost
    {
        public IUnityContainer Container
        { get; private set; }


        private void ApplyServiceBehaviors()
        {
            var behaviors = Container.ResolveAll<IServiceBehavior>();
            foreach (var behavior in behaviors)
                Description.Behaviors.Add(behavior);
        }


        private void ApplyContractBehaviors()
        {
            foreach (var contractDescription in ImplementedContracts.Values)
            {
                var behaviors = Container.ResolveAll<IContractBehavior>();
                foreach (var behavior in behaviors)
                    contractDescription.Behaviors.Add(behavior);
            }
        }


        private void ApplyEndpointBehaviors()
        {
            var registeredBehaviors = Container.ResolveAll<IEndpointBehavior>();

            foreach (var behavior in registeredBehaviors)
                foreach (var endpoint in Description.Endpoints)
                    endpoint.Behaviors.Add(behavior);
        }
        

        public UnityWebServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            Guard.AgainstNull(container, "container");
            Container = container;

            ApplyServiceBehaviors();
            ApplyContractBehaviors();
            ApplyEndpointBehaviors();
        }
    }
}