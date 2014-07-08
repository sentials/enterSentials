using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    internal class PerServiceOperationContextLifetimeManager : ExtensionBasedLifetimeManager<OperationContext>
    {
        protected override OperationContext GetExtensibleObject()
        { return Current.Service.Operation.Context; }
    }
}
