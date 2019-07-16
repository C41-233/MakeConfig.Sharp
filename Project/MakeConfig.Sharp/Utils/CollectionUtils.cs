using System;
using System.Collections.Generic;

namespace MakeConfig.Utils
{
    internal static class CollectionUtils
    {

        public static V GetValueOrCreate<K, V>(this Dictionary<K, V> dictionary, K key) where V : new()
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = new V();
                dictionary.Add(key, value);
                return value;
            }

            return value;
        }

        public static bool TryGetValue<V>(this IEnumerable<V> self, Predicate<V> predicate, out V value)
        {
            foreach (var e in self)
            {
                if (predicate(e))
                {
                    value = e;
                    return true;
                }    
            }

            value = default;
            return false;
        }

    }

}
