using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public class IocSupportingServiceBehavior : IServiceBehavior
    {
        private class IocSupportingDispatchMessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
            {
                Current.Service.Operation.Context.AttachComponentContainer();
                return null;
            }

            public void BeforeSendReply(ref Message reply, object correlationState)
            { Current.Service.Operation.Context.DetachComponentContainer(); }
        }


        private class IocSupportingCallContextInitializer : ICallContextInitializer
        {
            public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
            {
                Guard.AgainstNull(channel, "channel");
                channel.AttachComponentContainer<IContextChannel>();
                return null;
            }

            public void AfterInvoke(object correlationState)
            { Current.Service.Operation.Channel.DetachComponentContainer(); }
        }


        private class IocSupportingInstanceContextContextInitializer : IInstanceContextInitializer
        {
            public void Initialize(InstanceContext instanceContext, Message message)
            {
                Guard.AgainstNull(instanceContext, "instanceContext");
                instanceContext.AttachComponentContainer(); // will automatically detach for this type of object
            }
        }



        private readonly IServiceInstanceProviderFactory serviceInstanceProviderFactory = null;

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHost)
        {
            Guard.AgainstNull(serviceDescription, "serviceDescription");
            Guard.AgainstNull(serviceHost, "serviceHost");

            serviceHost.AttachComponentContainer(); // will automatically detach for this type of object

            var endpointDispatchers = serviceHost.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany(channelDispatcher => channelDispatcher.Endpoints);
            foreach (var endpointDispatcher in endpointDispatchers)
            {
                endpointDispatcher.DispatchRuntime.InstanceProvider = serviceInstanceProviderFactory.GetInstanceProviderFor(serviceDescription.ServiceType);
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new IocSupportingDispatchMessageInspector());
                endpointDispatcher.DispatchRuntime.InstanceContextInitializers.Add(new IocSupportingInstanceContextContextInitializer());
                endpointDispatcher.DispatchRuntime.Operations.ForEach(operation => operation.CallContextInitializers.Add(new IocSupportingCallContextInitializer()));
            }
        }


        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { }


        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        { }


        public IocSupportingServiceBehavior(IServiceInstanceProviderFactory serviceInstanceProviderFactory)
        {
            Guard.AgainstNull(serviceInstanceProviderFactory, "serviceInstanceProviderFactory");
            this.serviceInstanceProviderFactory = serviceInstanceProviderFactory;
        }
    }
}
