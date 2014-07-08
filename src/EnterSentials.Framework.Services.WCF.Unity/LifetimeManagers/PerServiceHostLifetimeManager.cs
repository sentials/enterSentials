using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    internal class PerServiceHostLifetimeManager : ExtensionBasedLifetimeManager<ServiceHostBase>
    {
        protected override ServiceHostBase GetExtensibleObject()
        { return Current.Service.Host; }
    }
}
