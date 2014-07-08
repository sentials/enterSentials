using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF.Unity
{
    internal class PerServiceChannelContextLifetimeManager : ExtensionBasedLifetimeManager<IContextChannel>
    {
        protected override IContextChannel GetExtensibleObject()
        { return Current.Service.Operation.Channel; }
    }
}
