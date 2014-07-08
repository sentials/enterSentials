using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class CorsPreflightRequestHandlingMessageFormatter : IDispatchMessageFormatter
    {
        private static readonly string CorsPropertyName = CorsEnablingMessageInspector.CorsPropertyName;

        private readonly IDispatchMessageFormatter originalFormatter = null;

        
        public void DeserializeRequest(Message message, object[] parameters)
        {
            var shouldDoNormalDeserialization = true;

            var corsStateObject = (object)null;
            if (message.Properties.TryGetValue(CorsPropertyName, out corsStateObject))
            {
                var state = corsStateObject as CorsState;
                if ((state != null) && (state.Message != null))
                {
                    OperationContext.Current.OutgoingMessageProperties.Add(CorsPropertyName, state);
                    shouldDoNormalDeserialization = false;
                }
            }

            if (shouldDoNormalDeserialization)
                originalFormatter.DeserializeRequest(message, parameters);
        }
        

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            var shouldDoNormalSerialization = true;

            var message = (Message)null;

            var corsStateObject = (object)null;
            if (OperationContext.Current.OutgoingMessageProperties.TryGetValue(CorsPropertyName, out corsStateObject))
            {
                var state = corsStateObject as CorsState;
                if ((state != null) && (state.Message != null))
                {
                    message = state.Message;
                    shouldDoNormalSerialization = false;
                }
            }

            if (shouldDoNormalSerialization)
                message = originalFormatter.SerializeReply(messageVersion, parameters, result);

            return message;
        }


        public CorsPreflightRequestHandlingMessageFormatter(IDispatchMessageFormatter formatter)
        {
            Guard.AgainstNull(formatter, "formatter");
            this.originalFormatter = formatter;
        }
    }
}
