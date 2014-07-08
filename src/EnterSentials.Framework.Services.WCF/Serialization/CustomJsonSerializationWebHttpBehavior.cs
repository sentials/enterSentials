using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace EnterSentials.Framework.Services.WCF
{
    public class CustomJsonSerializationWebHttpBehavior : WebHttpBehavior
    {
        public override void Validate(ServiceEndpoint endpoint)
        {
            base.Validate(endpoint);

            var elements = endpoint.Binding.CreateBindingElements();
            var webEncoder = elements.Find<WebMessageEncodingBindingElement>();

            Guard.Against(
                webEncoder == null, 
                "This behavior must be used in an endpoint with the WebHttpBinding (or a custom binding with the WebMessageEncodingBindingElement).");

            foreach (OperationDescription operation in endpoint.Contract.Operations)
                this.ValidateOperation(operation);
        }


        protected override IDispatchMessageFormatter GetRequestDispatchFormatter(
            OperationDescription operationDescription, 
            ServiceEndpoint endpoint)
        {
            return (this.IsGetOperation(operationDescription))
                ? base.GetRequestDispatchFormatter(operationDescription, endpoint)
                : (operationDescription.Messages[0].Body.Parts.Count == 0)
                    ? base.GetRequestDispatchFormatter(operationDescription, endpoint)
                    : new CustomJsonSerializationDispatchMessageFormatter(operationDescription, true);
        }


        protected override IDispatchMessageFormatter GetReplyDispatchFormatter(
            OperationDescription operationDescription, 
            ServiceEndpoint endpoint)
        {
            return (operationDescription.Messages.Count == 1 || operationDescription.Messages[1].Body.ReturnValue.Type == typeof(void))
                ? base.GetReplyDispatchFormatter(operationDescription, endpoint)
                : new CustomJsonSerializationDispatchMessageFormatter(operationDescription, false);
        }


        protected override IClientMessageFormatter GetRequestClientFormatter(
            OperationDescription operationDescription, 
            ServiceEndpoint endpoint)
        {
            var formatter = (IClientMessageFormatter)null;

            if (operationDescription.Behaviors.Find<WebGetAttribute>() != null)
                formatter = base.GetRequestClientFormatter(operationDescription, endpoint);
            else
            {
                var wia = operationDescription.Behaviors.Find<WebInvokeAttribute>();
                if ((wia != null) && (string.Equals(wia.Method, Http.Method.Head, StringComparison.InvariantCultureIgnoreCase)))
                    formatter = base.GetRequestClientFormatter(operationDescription, endpoint);
            }

            if ((formatter == null) && (operationDescription.Messages[0].Body.Parts.Count == 0))
                formatter = base.GetRequestClientFormatter(operationDescription, endpoint);

            if (formatter == null)
                formatter = new CustomJsonSerializationClientMessageFormatter(operationDescription, endpoint);

            return formatter;
        }


        protected override IClientMessageFormatter GetReplyClientFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            return (operationDescription.Messages.Count == 1 || operationDescription.Messages[1].Body.ReturnValue.Type == typeof(void))
                ? base.GetReplyClientFormatter(operationDescription, endpoint)
                : new CustomJsonSerializationClientMessageFormatter(operationDescription, endpoint);
        }


        private void ValidateOperation(OperationDescription operation)
        {
            if (operation.Messages.Count > 1)
            {
                Guard.Against(
                    operation.Messages[1].Body.Parts.Count > 0,
                    "Operations cannot have out/ref parameters.");
            }


            var uriTemplate = this.GetUriTemplate(operation);
            Guard.Against(
                uriTemplate != null,
                "UriTemplate support not implemented in this behavior.");


            var bodyStyle = this.GetBodyStyle(operation);
            var inputParameterCount = operation.Messages[0].Body.Parts.Count;

            if (!this.IsGetOperation(operation))
            {
                var wrappedRequest = bodyStyle == WebMessageBodyStyle.Wrapped || bodyStyle == WebMessageBodyStyle.WrappedRequest;

                Guard.Against(
                    inputParameterCount == 1 && wrappedRequest,
                    "Wrapped body style for single parameters not implemented in this behavior.");
            }


            var wrappedResponse = bodyStyle == WebMessageBodyStyle.Wrapped || bodyStyle == WebMessageBodyStyle.WrappedResponse;
            var isVoidReturn = operation.Messages.Count == 1 || operation.Messages[1].Body.ReturnValue.Type == typeof(void);
            Guard.Against(
                !isVoidReturn && wrappedResponse,
                "Wrapped response not implemented in this behavior.");
        }


        private string GetUriTemplate(OperationDescription operation)
        {
            var wga = (WebGetAttribute) null;
            var wia = (WebInvokeAttribute) null;

            return ((wga = operation.Behaviors.Find<WebGetAttribute>()) != null)
                ? wga.UriTemplate
                : ((wia = operation.Behaviors.Find<WebInvokeAttribute>()) != null)
                    ? wia.UriTemplate
                    : null;
        }


        private WebMessageBodyStyle GetBodyStyle(OperationDescription operation)
        {
            var wga = (WebGetAttribute)null;
            var wia = (WebInvokeAttribute)null;

            return ((wga = operation.Behaviors.Find<WebGetAttribute>()) != null)
                ? wga.BodyStyle
                : ((wia = operation.Behaviors.Find<WebInvokeAttribute>()) != null)
                    ? wia.BodyStyle
                    : this.DefaultBodyStyle;
        }


        private bool IsGetOperation(OperationDescription operation)
        {
            var wga = (WebGetAttribute) null;
            var wia = (WebInvokeAttribute) null;

            return ((wga = operation.Behaviors.Find<WebGetAttribute>()) != null)
                ? true
                : ((wia = operation.Behaviors.Find<WebInvokeAttribute>()) != null)
                    ? string.Equals(wia.Method, Http.Method.Head, StringComparison.InvariantCultureIgnoreCase)
                    : false;
        }
    }
}