using System.Collections.Generic;

namespace ByteRush.Utilities.Extensions
{
    public static class IDictionaryExtensions
    {
        public static void AddOrUpdate<K, V>(this IDictionary<K, V> self, K key, V newValue)
        {
            if (self.ContainsKey(key))
            {
                self[key] = newValue;
            }
            else
            {
                self.Add(key, newValue);
            }
        }

        public static V GetOrAdd<K, V>(this IDictionary<K, V> self, K key, V newValue)
        {
            if (self.TryGetValue(key, out var value)) return value;
            self[key] = newValue;
            return newValue;
        }
    }
}
