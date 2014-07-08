using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace EnterSentials.Framework.Services.WCF
{
    public class ApplyCorsConfigurationEndpointBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        { get { return typeof(ApplyCorsConfigurationEndpointBehavior); } }


        protected override object CreateBehavior()
        { return new ApplyCorsConfigurationEndpointBehavior(); }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { }
        
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var corsOperations = endpoint.Contract.Operations
               .Where(o => o.Behaviors.Find<EnableCorsAttribute>() != null)
               .ToArray();

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CorsEnablingMessageInspector(corsOperations));
        }

        public void Validate(ServiceEndpoint endpoint)
        { }
    }
}
