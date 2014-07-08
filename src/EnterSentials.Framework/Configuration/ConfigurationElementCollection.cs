using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace EnterSentials.Framework
{
    public abstract class ConfigurationElementCollection<TElement> : ConfigurationElementCollection, ICollection<TElement> where TElement : ConfigurationElement
    {
        protected override bool ThrowOnDuplicate
        { get { return true; } }

        public override ConfigurationElementCollectionType CollectionType
        { get { return ConfigurationElementCollectionType.BasicMap; } }

        public TElement this[int index]
        { get { return Get(index); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public TElement this[object key]
        { get { return Get(key); } }


        public bool TryGet(object key, out TElement element)
        {
            element = BaseGet(key) as TElement;
            return element != null;
        }

        public bool Contains(object key)
        {
            var ignored = (TElement)null;
            return TryGet(key, out ignored);
        }

        public bool Contains(TElement item)
        { return base.BaseIndexOf(item) != -1; }

        public TElement Get(int index)
        { return (TElement)base.BaseGet(index); }

        public virtual TElement Get(object key)
        {
            var element = (TElement)null;
            TryGet(key, out element);
            return element;
        }

        public void Add(TElement element)
        { BaseAdd(element); }

        public virtual void Remove(object key)
        { BaseRemove(key); }

        public bool Remove(TElement item)
        {
            var index = base.BaseIndexOf(item);
            var canOrNot = index > -1;
            if (canOrNot)
                BaseRemoveAt(index);
            return canOrNot;
        }

        public void Clear()
        { BaseClear(); }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            Guard.AgainstNull(array, "array");
            Guard.AgainstOutOfRange(arrayIndex, "arrayIndex");
            Guard.Against(array, a => (a.Length - arrayIndex) > Count, "The provided array and index does not provide sufficient space for copying.", "array");

            var i = arrayIndex;
            foreach (var element in this)
                array[i++] = element;
        }

        new public bool IsReadOnly
        { get { return base.IsReadOnly(); } }


        new public IEnumerator<TElement> GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
                yield return (TElement)enumerator.Current;
            yield break;
        }


        protected virtual TElement NewElement()
        { return Activator.CreateInstance<TElement>(); }

        sealed protected override ConfigurationElement CreateNewElement()
        { return NewElement(); }


        protected virtual object GetElementKey(TElement element)
        {
            var configurationProperties = typeof(TElement).PropertiesWithDeclaredOrInheritedAttributeOfType(typeof(ConfigurationPropertyAttribute));

            var keyProperty = configurationProperties.FirstOrDefault(property =>
            {
                var configurationPropertyAttributes = property.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), true).Cast<ConfigurationPropertyAttribute>();
                return configurationPropertyAttributes.Any() && configurationPropertyAttributes.First().IsKey;
            });

            return (keyProperty == null) ? null : keyProperty.GetValue(element, null);
        }


        sealed protected override object GetElementKey(ConfigurationElement element)
        { return GetElementKey((TElement)element); }


        public ConfigurationElementCollection()
        { }

        public ConfigurationElementCollection(TElement[] elements) : this((IEnumerable<TElement>)elements)
        { }

        public ConfigurationElementCollection(IEnumerable<TElement> elements)
        { elements.ForEach(Add); }
    }
}
