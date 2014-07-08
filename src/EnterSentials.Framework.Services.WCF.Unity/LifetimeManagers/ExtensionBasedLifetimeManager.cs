using EnterSentials.Framework.Unity;
using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    internal abstract class ExtensionBasedLifetimeManager<T> : ComponentContainerProxyBasedLifetimeManager where T : IExtensibleObject<T>
    {
        protected abstract T GetExtensibleObject();

        protected override ComponentContainer GetComponentContainer()
        { 
            var extensibleObject = GetExtensibleObject();
            var extension = extensibleObject.Extensions.Find<ComponentContainerExtension<T>>();
            Guard.Against(extension == null, "Appropriate WCF extensions must be applied to use WCF-based lifetime managers.");
            return extension.ComponentContainer;
        }
    }
}