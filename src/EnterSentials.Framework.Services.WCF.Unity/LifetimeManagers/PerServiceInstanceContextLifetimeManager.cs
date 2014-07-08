using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    internal class PerServiceInstanceContextLifetimeManager : ExtensionBasedLifetimeManager<InstanceContext>
    {
        protected override InstanceContext GetExtensibleObject()
        { return Current.Service.Operation.InstanceContext; }
    }
}
