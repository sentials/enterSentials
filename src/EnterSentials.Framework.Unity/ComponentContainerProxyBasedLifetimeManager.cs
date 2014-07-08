using Microsoft.Practices.Unity;
using System;

namespace EnterSentials.Framework.Unity
{
    public abstract class ComponentContainerProxyBasedLifetimeManager : LifetimeManager
    {
        private readonly Guid key = Guid.NewGuid();

        protected abstract ComponentContainer GetComponentContainer();


        public override object GetValue()
        { return GetComponentContainer().Resolve(key); }

        public override void RemoveValue()
        { GetComponentContainer().Release(key); }

        public override void SetValue(object newValue)
        { GetComponentContainer().Register(key, newValue); }
    }
}
