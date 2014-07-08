using System;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace EnterSentials.Framework.Services.WCF
{
    public class CustomJsonSerializationClientMessageFormatter : IClientMessageFormatter
    {
        private const string MessageBodyFormat = RawBodyWriter.BodyFormat;

        private readonly Uri operationUri = null;
        private readonly Type operationReturnType = null;
        private readonly string requestAction = null;
        private readonly SerializationParameterKeyedCollection operationParameters = null;
        
        
        public object DeserializeReply(Message message, object[] parameters) 
        {
            var bodyFormatProperty = (object)null;
            Guard.Against(
                !message.Properties.TryGetValue(WebBodyFormatMessageProperty.Name, out bodyFormatProperty)
                || (((WebBodyFormatMessageProperty)bodyFormatProperty).Format != WebContentFormat.Raw),
                "Incoming messages must have a body format of Raw. Is a ContentTypeMapper set on the WebHttpBinding?");

            var bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement(MessageBodyFormat);

            var rawBody = bodyReader.ReadContentAsBase64();
            return rawBody.DeserializeJsonTo(operationReturnType);
        }
        

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters) 
        {
            var arguments = operationParameters.Clone();
            foreach(var parameter in arguments)
                parameter.Value = parameters[parameter.Index];

            var body = arguments.SerializeAndWrapToBytes();

            var requestMessage = Message.CreateMessage(messageVersion, requestAction, new RawBodyWriter(body)); 
            requestMessage.Headers.To = operationUri;
            requestMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw)); 

            var httpRequest = new HttpRequestMessageProperty();
            httpRequest.Headers[HttpRequestHeader.ContentType] = ContentType.Json;

            requestMessage.Properties.Add(HttpRequestMessageProperty.Name, httpRequest); 

            return requestMessage; 
        }


        public CustomJsonSerializationClientMessageFormatter(OperationDescription operation, ServiceEndpoint endpoint)
        {
            Guard.AgainstNull(operation, "operation");
            Guard.AgainstNull(endpoint, "endpoint");

            this.operationReturnType = operation.GetReturnType();
            this.requestAction = operation.GetRequestAction();

            var endpointAddress = endpoint.Address.Uri.ToString();
            if (!endpointAddress.EndsWith("/"))
                endpointAddress = endpointAddress + "/";

            this.operationUri = new Uri(endpointAddress + operation.Name);
            this.operationParameters = operation.GetParameters();
        }
    }
}
