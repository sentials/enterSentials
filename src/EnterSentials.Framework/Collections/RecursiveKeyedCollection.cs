using System;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    [Serializable]
    public abstract class RecursiveKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
    {
        protected abstract bool ItemHasRecursiveReference(TItem item);
        protected abstract TKey GetRecursiveReferenceIdForItem(TItem item);
        protected abstract void AssignResursiveReferenceOnItem(TItem item, TItem referencedItem);

        public void FulfillRecursiveReferences()
        {
            foreach (var item in this)
            {
                if (ItemHasRecursiveReference(item))
                {
                    var referenceId = GetRecursiveReferenceIdForItem(item);
                    if (this.Contains(referenceId))
                        AssignResursiveReferenceOnItem(item, this[referenceId]);
                }
            }
        }
    }
}