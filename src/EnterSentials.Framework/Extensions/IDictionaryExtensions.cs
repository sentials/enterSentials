using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public static class IDictionaryExtensions
    {
        public static IDictionary<TKey, TValue> AddIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool condition)
        {
            if (condition)
                dictionary.Add(key, value);

            return dictionary;
        }

        public static IDictionary<TKey, TValue> RemoveIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, bool condition)
        {
            if (condition)
                dictionary.Remove(key);

            return dictionary;
        }


        public static IDictionary<TKey, TValue> AddIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TKey, TValue, bool> condition)
        { return dictionary.AddIf(key, value, condition(key, value)); }

        public static IDictionary<TKey, TValue> RemoveIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Predicate<TKey> condition)
        { return dictionary.RemoveIf(key, condition(key)); }

        public static IDictionary<TKey, TValue> AddIfNotExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        { return dictionary.AddIf(key, value, !dictionary.ContainsKey(key)); }

        public static IDictionary<TKey, TValue> RemoveIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        { return dictionary.RemoveIf(key, dictionary.ContainsKey(key)); }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        { return pairs.ToDictionary(entry => entry.Key, entry => entry.Value); }

        public static bool TryGetValue(this IDictionary dictionary, object key, out object value)
        {
            var couldOrNot = dictionary.Contains(key);
            value = couldOrNot ? dictionary[key] : null;
            return couldOrNot;
        }
    }
}
