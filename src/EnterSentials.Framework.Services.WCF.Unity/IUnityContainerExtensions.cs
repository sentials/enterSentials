using EnterSentials.Framework.Services.WCF;
using EnterSentials.Framework.Services.WCF.Unity;
using EnterSentials.Framework.Unity;
using Microsoft.Practices.Unity;
using System.ServiceModel.Description;

namespace EnterSentials.Framework
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer RegisterWcfServicesFrameworkComponents(this IUnityContainer container)
        {
            return container
                .RegisterType<IUnitOfWorkFactory, UnityBasedUnitOfWorkFactory>(new PerServiceOperationContextLifetimeManager())
                .RegisterType<IServiceInstanceProviderFactory, UnityServiceInstanceProviderFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IServiceBehavior, IocSupportingServiceBehavior>(typeof(IocSupportingServiceBehavior).Name);
        }
    }
}