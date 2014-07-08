using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnterSentials.Framework
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TElement> ForEach<TElement>(
            this IEnumerable<TElement> elements,
            Action<TElement> action
        )
        {
            if ((elements != null) && (action != null))
            {
                foreach (var element in elements)
                    action(element);
            }

            return elements;
        }


        public static IEnumerable<TElement> ForEachYield<TElement>(
            this IEnumerable<TElement> elements,
            Action<TElement> action
        )
        {
            if (elements != null)
            {
                if (action != null)
                {
                    foreach (var element in elements)
                    {
                        action(element);
                        yield return element;
                    }
                }
                else
                    foreach (var element in elements)
                        yield return element;
            }
            else
                yield break;
        }


        public static IEnumerable<T> ForAll<T>(
            this IEnumerable<T> elements,
            Predicate<T> actionCondition,
            Action<T> action
        )
        {
            Predicate<T> condition = actionCondition ?? (item => true);
            return elements.Where(element => condition(element)).ForEach(action);
        }


        public static IEnumerable<T> ForAllYield<T>(
            this IEnumerable<T> elements,
            Predicate<T> actionCondition,
            Action<T> action
        )
        {
            Predicate<T> condition = actionCondition ?? (item => true);
            return elements.Where(element => condition(element)).ForEachYield(action);
        }


        public static ICollection<TElement> ToCollection<TElement>(this IEnumerable<TElement> elements)
        {
            Guard.AgainstNull(elements, "elements");
            return elements.ToList();
        }

        public static Queue<TElement> ToQueue<TElement>(this IEnumerable<TElement> elements)
        {
            Guard.AgainstNull(elements, "elements");
            var elementsAsQueue = elements as Queue<TElement>;
            return elementsAsQueue ?? new Queue<TElement>(elements);
        }

        public static Stack<TElement> ToStack<TElement>(this IEnumerable<TElement> elements)
        {
            Guard.AgainstNull(elements, "elements");
            var elementsAsStack = elements as Stack<TElement>;
            return elementsAsStack ?? new Stack<TElement>(elements);
        }

        
        public static TKeyedCollection ToKeyedCollection<TKeyedCollection, TKey, TElement>(this IEnumerable<TElement> elements)
            where TKeyedCollection : KeyedCollection<TKey, TElement>, new()
        {
            Guard.AgainstNull(elements, "elements");

            var keyedCollection = elements as TKeyedCollection;
            if (keyedCollection == null)
            {
                keyedCollection = Activator.CreateInstance<TKeyedCollection>();
                elements.ForEach(keyedCollection.Add);
                var recursiveKeyedCollection = keyedCollection as RecursiveKeyedCollection<TKey, TElement>;
                var multiRecursiveKeyedCollection = keyedCollection as MultiRecursiveKeyedCollection<TKey, TElement>;
                if (recursiveKeyedCollection != null)
                    recursiveKeyedCollection.FulfillRecursiveReferences();
                else if (multiRecursiveKeyedCollection != null)
                    multiRecursiveKeyedCollection.FulfillRecursiveReferences();
            }

            return keyedCollection;
        }
    }
}
