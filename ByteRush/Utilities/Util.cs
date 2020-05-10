using ByteRush.Utilities.Extensions;
using System.Collections.Generic;

namespace ByteRush.Utilities
{
    public static class Util
    {
        public static T[] NewArray<T>(params T[] value) => value;

        public static Dictionary<K, V> NewDictionary<K, V>(params (K, V)[] value) => value.ToDictionary();

        public static HashSet<T> NewHashSet<T>(params T[] value) => new HashSet<T>(value);

        public static KeyValuePair<K, V> NewKVP<K, V>(K key, V value) => new KeyValuePair<K, V>(key, value);
    }
}
