using Microsoft.Practices.Unity;
using System;
using System.ServiceModel.Web;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    public abstract class UnityWebServiceHostFactory : WebServiceHostFactoryBase
    {
        protected virtual void ConfigureStaticContainer(IUnityContainer container)
        {
            container
                .RegisterCoreFrameworkComponents()
                .RegisterWcfServicesFrameworkComponents();
        }

        protected abstract void ConfigureContainerForServiceHost(IUnityContainer container);
        

        protected virtual void OnStaticContainerConfigured()
        { }
        
        protected virtual void OnContainerConfiguredForServiceHost()
        { }


        private void InternalOnStaticContainerConfigured(IUnityContainer container)
        {
            container.Resolve<IComponents>().ExecuteBootstrappers();
            OnStaticContainerConfigured();
        }
        
        private void InternalOnContainerConfiguredForServiceHost(IUnityContainer container)
        {
            OnContainerConfiguredForServiceHost();
        }

        
        protected override WebServiceHost NewServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Components.SetInstanceProviderIfNotAlreadySet(
                () =>
                {
                    var staticComponents = Components.GetSettingsBasedComponentsInstance();
                    var staticContainer = staticComponents.Get<IUnityContainer>();
                    ConfigureStaticContainer(staticContainer);
                    InternalOnStaticContainerConfigured(staticContainer);
                    return staticComponents;
                }
            );

            var hostContainer = Components.Instance.Get<IUnityContainer>().CreateChildContainer();
            ConfigureContainerForServiceHost(hostContainer);
            InternalOnContainerConfiguredForServiceHost(hostContainer);
            return new UnityWebServiceHost(hostContainer, serviceType, baseAddresses);
        }
    }
}