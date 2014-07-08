using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class CustomHeaderApplyingMessageInspector : IDispatchMessageInspector
    {
        private readonly IEnumerable<KeyValuePair<string, string>> headersToAdd = null;


        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        { return null; }


        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var httpResponse = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;

            if (httpResponse != null)
            {
                foreach (var item in headersToAdd)
                    httpResponse.Headers.Add(item.Key, item.Value);
            }
        }



        public CustomHeaderApplyingMessageInspector(IEnumerable<KeyValuePair<string, string>> headers)
        { this.headersToAdd = (headers == null) ? Enumerable.Empty<KeyValuePair<string, string>>() : headers; }
    }
}
