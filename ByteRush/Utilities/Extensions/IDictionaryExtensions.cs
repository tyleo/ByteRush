using System.Collections.Generic;

namespace ByteRush.Utilities.Extensions
{
    public static class IDictionaryExtensions
    {
        public static V GetOrAdd<K, V>(this IDictionary<K, V> self, K key, V newValue)
        {
            if (self.TryGetValue(key, out var value)) return value;
            self[key] = newValue;
            return newValue;
        }
    }
}
