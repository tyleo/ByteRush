using ByteRush.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities.Extensions
{
    public static class IEnumerableExt
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] items) =>
            items.SelectMany(i => i);

        public static IEnumerable<T> Default<T>(int length)
        {
            for (var i = 0; i < length; ++i) yield return default;
        }

        private sealed class MapEqualityComparer<T, U> : IEqualityComparer<T>
        {
            private readonly Func<T, U> _map;
            private readonly IEqualityComparer<U> _defaultComparer;

            public MapEqualityComparer(Func<T, U> map)
            {
                _map = map;
                _defaultComparer = EqualityComparer<U>.Default;
            }

            public bool Equals(T x, T y) => _defaultComparer.Equals(_map(x), _map(y));

            public int GetHashCode(T obj) => _defaultComparer.GetHashCode(_map(obj));
        }

        public static IEnumerable<T> Distinct<T, U>(this IEnumerable<T> self, Func<T, U> map) =>
            self.Distinct(new MapEqualityComparer<T, U>(map));

        public static IEnumerable<int> Enumerate(this int self) =>
            Enumerable.Range(0, self);

        public static IEnumerable<(T Value, int Index)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((o, i) => (o, i));

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> op)
        {
            foreach (var i in self) op(i);
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<(K, V)> self) =>
            self.ToDictionary(i => i.Item1, i => i.Item2);

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> self) => new HashSet<T>(self);

        public static IRefEnumerable<T> IntoRef<T>(this IEnumerable<T> self) =>
            RefEnumerableAdapter<T>.New(self);

        public static IEnumerable<(T, U)> Zip<T, U>(this IEnumerable<T> lhs, IEnumerable<U> rhs) =>
            lhs.Zip(rhs, (lItem, rItem) => (lItem, rItem));
    }
}
