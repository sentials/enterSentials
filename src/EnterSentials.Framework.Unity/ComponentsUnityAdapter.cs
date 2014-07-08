using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace EnterSentials.Framework.Unity
{
    public class ComponentsUnityAdapter : IComponents
    {
        private readonly IUnityContainer container = null;


        public IEnumerable<T> GetAll<T>()
        { return container.ResolveAll<T>(); }

        public IEnumerable<object> GetAll(Type type)
        { return container.ResolveAll(type); }

        public T Get<T>(string key)
        { return container.Resolve<T>(key); }

        public T Get<T>()
        { return container.Resolve<T>(); }

        public object Get(Type type, string key)
        { return container.Resolve(type, key); }

        public object Get(Type type)
        { return container.Resolve(type); }


        public ComponentsUnityAdapter(IUnityContainer container)
        {
            Guard.AgainstNull(container, "container");
            this.container = container;
        }
    }
}