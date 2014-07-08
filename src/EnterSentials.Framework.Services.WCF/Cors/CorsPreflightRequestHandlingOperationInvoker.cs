using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class CorsPreflightRequestHandlingOperationInvoker : IOperationInvoker
    {
        private static readonly string CorsPropertyName = CorsEnablingMessageInspector.CorsPropertyName;

        private readonly IOperationInvoker originalInvoker = null;


        public bool IsSynchronous
        { get { return originalInvoker.IsSynchronous; } }

        
        public object[] AllocateInputs()
        { return originalInvoker.AllocateInputs(); }


        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = null;

            var shouldDoNormalInvocation = true;
            var result = (object)null;

            var corsStateObject = (object)null;
            if (OperationContext.Current.IncomingMessageProperties.TryGetValue(CorsPropertyName, out corsStateObject))
            {
                var state = corsStateObject as CorsState;
                if ((state != null) && (state.Message != null))
                    shouldDoNormalInvocation = false;
            }

            if (shouldDoNormalInvocation)
                result = originalInvoker.Invoke(instance, inputs, out outputs);

            return result;
        }


        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        { throw new NotSupportedException(); }


        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        { throw new NotSupportedException(); }


        public CorsPreflightRequestHandlingOperationInvoker(IOperationInvoker originalInvoker)
        {
            Guard.AgainstNull(originalInvoker, "innerInvoker");
            Guard.Against(originalInvoker, i => !i.IsSynchronous, "This implementation only supports synchronous operation invokers.", "innerInvoker");

            this.originalInvoker = originalInvoker;
        }
    }
}