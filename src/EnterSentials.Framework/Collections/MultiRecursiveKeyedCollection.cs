using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnterSentials.Framework
{
    [Serializable]
    public abstract class MultiRecursiveKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
    {
        protected abstract bool ItemHasRecursiveReferences(TItem item);
        protected abstract IEnumerable<TKey> GetRecursiveReferenceIdsForItem(TItem item);
        protected abstract void AssignResursiveReferencesOnItem(TItem item, IEnumerable<TItem> referencedItems);

        public void FulfillRecursiveReferences()
        {
            foreach (var item in this)
            {
                if (ItemHasRecursiveReferences(item))
                {
                    var referenceIds = GetRecursiveReferenceIdsForItem(item);

                    foreach (var referenceId in referenceIds)
                    {
                        AssignResursiveReferencesOnItem(
                            item, 
                            this.Where(i => referenceIds.Contains(GetKeyForItem(i))).ToArray()
                        );
                    }
                }
            }
        }
    }
}
