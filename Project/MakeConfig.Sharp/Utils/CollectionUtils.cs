
using System.Collections.Generic;

namespace MakeConfig.Utils
{
    public static class CollectionUtils
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

    }

}
