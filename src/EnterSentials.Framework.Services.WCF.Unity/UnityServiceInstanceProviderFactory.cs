using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    public class UnityServiceInstanceProviderFactory : IServiceInstanceProviderFactory
    {
        private class UnityServiceInstanceProvider : IInstanceProvider
        {
            private readonly IUnityContainer container = null;
            private readonly Type serviceType = null;


            public object GetInstance(InstanceContext instanceContext, Message message)
            { return container.Resolve(serviceType); }

            public object GetInstance(InstanceContext instanceContext)
            { return GetInstance(instanceContext, null); }

            public void ReleaseInstance(InstanceContext instanceContext, object instance)
            { container.Teardown(instance); }


            public UnityServiceInstanceProvider(IUnityContainer container, Type serviceType)
            {
                Guard.AgainstNull(container, "container");
                Guard.AgainstNull(serviceType, "serviceType");
                this.container = container;
                this.serviceType = serviceType;
            }
        }



        private readonly IDictionary<Type, IInstanceProvider> instanceProviders = new Dictionary<Type, IInstanceProvider>();
        private readonly IUnityContainer container = null;

        public IInstanceProvider GetInstanceProviderFor(Type serviceType)
        {
            lock (instanceProviders)
            {
                var instanceProvider = (IInstanceProvider)null;
                if (!instanceProviders.TryGetValue(serviceType, out instanceProvider))
                    instanceProvider = instanceProviders[serviceType] = new UnityServiceInstanceProvider(container, serviceType);
                return instanceProvider;
            }
        }

        public IInstanceProvider GetInstanceProviderFor<TService>()
        { return GetInstanceProviderFor(typeof(TService)); }


        public UnityServiceInstanceProviderFactory(IUnityContainer container)
        {
            Guard.AgainstNull(container, "container");
            this.container = container;
        }
    }
}
