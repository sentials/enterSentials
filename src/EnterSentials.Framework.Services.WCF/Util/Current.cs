using System.ServiceModel;
using System.ServiceModel.Web;

namespace EnterSentials.Framework.Services.WCF
{
    public static class Current
    {
        public static class Service
        {
            public static class Operation
            {
                public static OperationContext Context
                { get { return OperationContext.Current; } }

                public static IContextChannel Channel
                { get { return Context.Channel; } }

                public static InstanceContext InstanceContext
                { get { return Context.InstanceContext; } }
            }

            public static class WebOperation
            {
                public static WebOperationContext Context
                { get { return WebOperationContext.Current; } }

                public static class Incoming
                { 
                    public static IncomingWebRequestContext Request
                    { get { return Context.IncomingRequest; } }

                    public static IncomingWebResponseContext Response
                    { get { return Context.IncomingResponse; } }
                }

                public static class Outgoing
                {
                    public static OutgoingWebRequestContext Request
                    { get { return Context.OutgoingRequest; } }

                    public static OutgoingWebResponseContext Response
                    { get { return Context.OutgoingResponse; } }
                }
            }

            public static ServiceHostBase Host
            { get { return Operation.Context.Host; } }
        }
    }
}