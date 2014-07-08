using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class EnableCorsEndpointBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        private static readonly IEnumerable<KeyValuePair<string, string>> CorsRequiredResponseHeaders = new Dictionary<string, string>()
            {
                { Http.Header.AccessControl.Allow.Origin, Http.Header.AccessControl.Allow.AnyOrigin },
                { Http.Header.AccessControl.Request.Method, Cors.RequiredAllowedMethods },
                { Http.Header.AccessControl.Allow.Methods, Cors.RequiredAllowedMethods },
                { Http.Header.AccessControl.Allow.Headers, Cors.RequiredAllowedHeaders },
            };


        public override Type BehaviorType
        { get { return typeof(EnableCorsEndpointBehavior); } }


        protected override object CreateBehavior()
        { return new EnableCorsEndpointBehavior(); }


        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var headers = CorsRequiredResponseHeaders.ToDictionary();
            
            if (Cors.ShouldIncludeNoCacheHeader)
                headers[Http.Header.CacheControl] = Http.Header.NoCache;

            if (Cors.ShouldIncludePreflightResponseMaxAgeHeader)
                headers[Http.Header.AccessControl.MaxAge] = Cors.PreflightResponseMaxAgeInSeconds.ToString();

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CustomHeaderApplyingMessageInspector(headers.ToArray()));

            
            endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.Invoker =
                new PreflightRequestHandlingOperationInvoker(endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.Invoker);
        }


        public void Validate(ServiceEndpoint endpoint)
        {
            Guard.Against(!(endpoint.Binding is WebHttpBinding), "Behavior \"" + typeof(EnableCorsEndpointBehavior).Name + "\" can only be applied to WebHttpBinding endpoints.");
        }
    }
}
