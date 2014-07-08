using System;
using System.ServiceModel.Dispatcher;

namespace EnterSentials.Framework.Services.WCF
{
    public interface IServiceInstanceProviderFactory
    {
        IInstanceProvider GetInstanceProviderFor(Type serviceType);
        IInstanceProvider GetInstanceProviderFor<TService>();
    }
}
