using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace EnterSentials.Framework.Services.WCF
{
    public class ComponentContainerExtension<TExtensibleObject> : IExtension<TExtensibleObject> where TExtensibleObject : IExtensibleObject<TExtensibleObject>
    {
        private ComponentContainer components = new ComponentContainer(new SortedDictionary<Guid, object>());


        public ComponentContainer ComponentContainer
        { get { return components; } }


        private void OnCommunicationObjectClosing(object sender, EventArgs e)
        {
            var communicationObject = sender as ICommunicationObject;
            if (communicationObject != null)
            {
                UnwireForExtensionRemoval(communicationObject);
                ((TExtensibleObject)communicationObject).DetachComponentContainer();
            }
        }


        private void WireUpForExtensionRemoval(ICommunicationObject communicationObject)
        {
            communicationObject.Closing -= OnCommunicationObjectClosing;
            communicationObject.Closing += OnCommunicationObjectClosing;
        }

        private void UnwireForExtensionRemoval(ICommunicationObject communicationObject)
        { communicationObject.Closing -= OnCommunicationObjectClosing; }


        internal void RemoveFrom(TExtensibleObject @object)
        { @object.TryRemoveComponentContainerExtension(); }


        public void Attach(TExtensibleObject owner)
        {
            var communicationObject = owner as ICommunicationObject;
            if (communicationObject != null)
                WireUpForExtensionRemoval(communicationObject);
        }

        public void Detach(TExtensibleObject owner)
        { components.ReleaseAll(); }
    }
}
