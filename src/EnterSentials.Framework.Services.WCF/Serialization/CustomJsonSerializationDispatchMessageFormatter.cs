using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class CustomJsonSerializationDispatchMessageFormatter : IDispatchMessageFormatter
    {
        private const string MessageBodyFormat = RawBodyWriter.BodyFormat;

        private readonly string responseAction = null;
        private readonly SerializationParameterKeyedCollection operationParameters = null;
        
        
        public void DeserializeRequest(Message message, object[] parameters) 
        { 
            var bodyFormatProperty = (object) null; 
            Guard.Against(
                !message.Properties.TryGetValue(WebBodyFormatMessageProperty.Name, out bodyFormatProperty)
                || (((WebBodyFormatMessageProperty)bodyFormatProperty).Format != WebContentFormat.Raw),
                "Incoming messages must have a body format of Raw. Is a ContentTypeMapper set on the WebHttpBinding?");
            
            var bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement(MessageBodyFormat);
            
            var rawBody = bodyReader.ReadContentAsBase64();
            rawBody.DeserializeWrappedIntoParameters(operationParameters, parameters);
        }
        

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result) 
        {
            var body = result.SerializeToJsonBytes();

            var replyMessage = Message.CreateMessage(messageVersion, responseAction, new RawBodyWriter(body)); 
            replyMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw));

            var httpResponse = new HttpResponseMessageProperty();
            httpResponse.Headers[HttpResponseHeader.ContentType] = ContentType.Json; 

            replyMessage.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);

            return replyMessage; 
        }




        public CustomJsonSerializationDispatchMessageFormatter(OperationDescription operation, bool isRequest)
        {
            Guard.AgainstNull(operation, "operation");
            this.responseAction = operation.GetResponseAction();
            this.operationParameters = isRequest ? operation.GetParameters() : new SerializationParameterKeyedCollection();
        } 
    }
}