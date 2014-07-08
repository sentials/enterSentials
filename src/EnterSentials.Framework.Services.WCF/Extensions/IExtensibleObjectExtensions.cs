using System;
using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF
{
    public static class IExtensibleObjectExtensions
    {
        public static bool TryAdd<TExtensibleObject, TExtension>(this IExtensionCollection<TExtensibleObject> extensions)
            where TExtensibleObject : IExtensibleObject<TExtensibleObject>
            where TExtension : IExtension<TExtensibleObject>
        {
            var couldOrNot = false;
            var extension = extensions.Find<TExtension>();
            if (extension == null)
            {
                extensions.Add(Activator.CreateInstance<TExtension>());
                couldOrNot = true;
            }

            return couldOrNot;
        }

        public static bool TryRemove<TExtensibleObject, TExtension>(this IExtensionCollection<TExtensibleObject> extensions)
            where TExtensibleObject : IExtensibleObject<TExtensibleObject>
            where TExtension : IExtension<TExtensibleObject>
        {
            var couldOrNot = false;
            var extension = extensions.Find<TExtension>();
            if (extension != null)
                couldOrNot = extensions.Remove(extension);
            return couldOrNot;
        }


        internal static void TryAddComponentContainerExtension<TExtensibleObject>(this TExtensibleObject @object)
            where TExtensibleObject : IExtensibleObject<TExtensibleObject>
        { @object.Extensions.TryAdd<TExtensibleObject, ComponentContainerExtension<TExtensibleObject>>(); }

        internal static void TryRemoveComponentContainerExtension<TExtensibleObject>(this TExtensibleObject @object)
            where TExtensibleObject : IExtensibleObject<TExtensibleObject>
        { @object.Extensions.TryRemove<TExtensibleObject, ComponentContainerExtension<TExtensibleObject>>(); }


        internal static void AttachComponentContainer<TExtensibleObject>(this TExtensibleObject @object)
            where TExtensibleObject : IExtensibleObject<TExtensibleObject>
        { @object.TryAddComponentContainerExtension(); }

        internal static void DetachComponentContainer<TExtensibleObject>(this TExtensibleObject @object)
            where TExtensibleObject : IExtensibleObject<TExtensibleObject>
        {
            var extension = @object.Extensions.Find<ComponentContainerExtension<TExtensibleObject>>();
            if (extension != null)
                extension.RemoveFrom(@object);
        }
    }
}
