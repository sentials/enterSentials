using System;

namespace EnterSentials.Framework.EmitMapper
{
    internal static class TypeDictionaryExtensions
    {
        public static bool TryGetValue<T>(this TypeDictionary<T> dictionary, Type[] types, out T entry) where T : class
        {
            Guard.AgainstNull(dictionary, "dictionary");
            Guard.AgainstNull(types, "types");
            entry = dictionary.GetValue(types);
            return entry != null;
        }
    }
}