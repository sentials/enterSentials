using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace EnterSentials.Framework.Services.WCF
{
    public class CorsEnablingMessageInspector : IDispatchMessageInspector
    {
        internal const string CorsPropertyName = "CorsState";


        private readonly ServiceEndpoint serviceEndpoint = null;
        private readonly ICorsConfiguration corsConfiguration = null;
            

        private string FindReplyAction(string requestAction) {
            var action = (string) null;

			foreach (var operation in serviceEndpoint.Contract.Operations) {
				if (operation.Messages[0].Action == requestAction) {
                    action = operation.Messages[1].Action;
                    break;
				}
			}

			return action;
		}


        private bool IsPreflight(HttpRequestMessageProperty httpRequest)
        { return string.Equals(httpRequest.Method, Http.Method.Options, StringComparison.InvariantCultureIgnoreCase); }


        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var state = (CorsState)null;

            var httpRequest = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;

            if (httpRequest != null) 
            {
                var origin = httpRequest.Headers[Http.Header.Origin];
                if (!string.IsNullOrEmpty(origin))
                {
                    state = new CorsState();

                    if (IsPreflight(httpRequest))
                        state.Message = Message.CreateMessage(request.Version, FindReplyAction(request.Headers.Action), new InertBodyWriter());

                    request.Properties.Add(CorsPropertyName, state);
                }
            }

            return state;
        }


        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var state = correlationState as CorsState;

			if (state != null) {
				if (state.Message != null)
					reply = state.Message;
				
				var httpResponse = (HttpResponseMessageProperty) null;
                
                var property = (object)null;
                if (reply.Properties.TryGetValue(HttpResponseMessageProperty.Name, out property))
                    httpResponse = property as HttpResponseMessageProperty;
                if (httpResponse == null)
                    reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponse = new HttpResponseMessageProperty());

                httpResponse.Headers[Http.Header.AccessControl.Allow.Origin] = corsConfiguration.AllowedOrigin;

                if (Cors.ShouldIncludeNoCacheHeader)
                    httpResponse.Headers[Http.Header.CacheControl] = Http.Header.NoCache;

				if (state.Message != null) 
                {
					httpResponse.Headers[Http.Header.AccessControl.Allow.Methods] = corsConfiguration.AllowedMethods;
					httpResponse.Headers[Http.Header.AccessControl.Allow.Headers] = corsConfiguration.AllowedHeaders;
                    
                    if (Cors.ShouldIncludePreflightResponseMaxAgeHeader)
                        httpResponse.Headers[Http.Header.AccessControl.MaxAge] = Cors.PreflightResponseMaxAgeInSeconds.ToString();
                }
            }
        }


        public CorsEnablingMessageInspector(ServiceEndpoint serviceEndpoint, ICorsConfiguration corsConfiguration)
        {
            Guard.AgainstNull(serviceEndpoint, "serviceEndpoint");
            Guard.AgainstNull(corsConfiguration, "corsConfiguration");

            this.serviceEndpoint = serviceEndpoint;
            this.corsConfiguration = corsConfiguration;
        }
    }
}