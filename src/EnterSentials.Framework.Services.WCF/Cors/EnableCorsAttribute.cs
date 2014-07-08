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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class EnableCorsAttribute : Attribute, IEndpointBehavior, IOperationBehavior, ICorsConfiguration
    {
        public string AllowedOrigin { get; set; }
        
        public string AllowedMethods { get; set; }

        public string AllowedHeaders { get; set; }


        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { }

        public void Validate(ServiceEndpoint endpoint)
        {
            Guard.Against(
                !(endpoint.Binding is WebHttpBinding), 
                "Behavior \"" + typeof(EnableCorsAttribute).Name + "\" can only be applied to WebHttpBinding endpoints.");
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        { endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CorsEnablingMessageInspector(endpoint, this)); }



        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        { }

        public void Validate(OperationDescription operationDescription)
        { }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Formatter = new CorsPreflightRequestHandlingMessageFormatter(dispatchOperation.Formatter);
            dispatchOperation.Invoker = new CorsPreflightRequestHandlingOperationInvoker(dispatchOperation.Invoker);
        }


        public EnableCorsAttribute()
        {
            AllowedOrigin = Cors.DefaultAllowedOrigin;
            AllowedMethods = Cors.DefaultAllowedMethods;
            AllowedHeaders = Cors.DefaultAllowedHeaders;
        }
    }
}