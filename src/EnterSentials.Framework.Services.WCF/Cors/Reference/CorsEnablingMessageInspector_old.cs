using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class CorsEnablingMessageInspector : IDispatchMessageInspector
    {
        private readonly IEnumerable<string> corsEnabledOperationNames = null;


        private readonly IDictionary<string, IEnumerable<ICorsConfiguration>> corsEnabledOperations = null;
        private IEnumerable<KeyValuePair<OperationDescription, IEnumerable<ICorsConfiguration>>> GetCorsEnabledOperationsOn(ServiceEndpoint endpoint)
        {
            // TODO: This is one place where things need to be changed to allow multiple specified origins per operation
            var corsEndpointBehaviors = endpoint.EndpointBehaviors.Where(b => b is EnableCorsAttribute).Cast<ICorsConfiguration>();
            return (corsEndpointBehaviors.Any()
                ? endpoint.Contract.Operations.Select(o => new KeyValuePair<OperationDescription, IEnumerable<ICorsConfiguration>>(o, corsEndpointBehaviors.ToArray()))
                : endpoint.Contract.Operations
                    .Where(o => o.OperationBehaviors.Any(b => b is EnableCorsAttribute))
                    .Select(o => new KeyValuePair<OperationDescription, IEnumerable<ICorsConfiguration>>(o, o.Behaviors
                        .Where(b => b is EnableCorsAttribute)
                        .Cast<ICorsConfiguration>()
                        .ToArray())))
            .ToArray();
        }




        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var origin = (object)null;
            var httpRequest = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;

            if (httpRequest != null) 
            { 
                var operationName = (object) null;
                if (request.Properties.TryGetValue(WebHttpDispatchOperationSelector.HttpOperationNamePropertyName, out operationName))
                {
                    var operationNameString = operationName as string;
                    if (!string.IsNullOrEmpty(operationNameString) && (corsEnabledOperationNames.Contains(operationNameString)))
                        origin = httpRequest.Headers[Http.Header.Origin];
                }
            }

            return origin;
        }


        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var origin = correlationState as string;

            if (!string.IsNullOrEmpty(origin))
            {
                var httpResponse = (HttpResponseMessageProperty) null;
                
                var property = (object)null;
                if (reply.Properties.TryGetValue(HttpResponseMessageProperty.Name, out property))
                    httpResponse = property as HttpResponseMessageProperty;
                else
                    reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponse = new HttpResponseMessageProperty());
                
                if (httpResponse != null)
                    httpResponse.Headers.Add(Http.Header.AccessControl.Allow.Origin, origin);
            }
        }



        public CorsEnablingMessageInspector(IEnumerable<OperationDescription> corsEnabledOperations)
        { 
            this.corsEnabledOperationNames = (corsEnabledOperations == null)
                ? Enumerable.Empty<string>() 
                : corsEnabledOperations.Select(o => o.Name).ToArray(); 
        }
    }
}
