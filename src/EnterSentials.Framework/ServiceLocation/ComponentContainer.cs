using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    sealed public class ComponentContainer
    {
        private readonly IDictionary components = null;
        private readonly ICollection<Guid> managedKeys = new Collection<Guid>();


        public void Register(Guid key, object component)
        {
            Guard.AgainstNull(component, "component");
            managedKeys.Add(key);
            components[key] = component;
        }

        public object Resolve(Guid key)
        {
            var component = (object)null;
            components.TryGetValue(key, out component);
            return component;
        }

        private bool TryDispose(Guid key)
        {
            var component = (object)null;
            var foundOrNot = default(bool);
            if (foundOrNot = components.TryGetValue(key, out component))
            {
                var disposable = component as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            return foundOrNot;
        }

        public void Release(Guid key)
        {
            if (TryDispose(key))
                components.Remove(key);
        }

        public void ReleaseAll()
        {
            foreach (var key in managedKeys)
                TryDispose(key);
            components.Clear();
        }


        public ComponentContainer(IDictionary internalComponentsStorage)
        {
            Guard.AgainstNull(internalComponentsStorage, "internalComponentsStorage");
            components = internalComponentsStorage;
        }
    }
}
