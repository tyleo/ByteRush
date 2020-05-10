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

        private class FuncEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> equalsImpl;
            private readonly Func<T, int> getHashCodeImpl;

            public bool Equals(T x, T y) => equalsImpl(x, y);

            public int GetHashCode(T obj) => getHashCodeImpl(obj);

            public FuncEqualityComparer(Func<T, T, bool> equalsImpl, Func<T, int> getHashCodeImpl)
            {
                this.equalsImpl = equalsImpl;
                this.getHashCodeImpl = getHashCodeImpl;
            }
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> self, Func<T, T, bool> equals, Func<T, int> getHashCode) =>
            self.Distinct(new FuncEqualityComparer<T>(equals, getHashCode));

        public static IEnumerable<T> Distinct<T, U>(this IEnumerable<T> self, Func<T, U> map) =>
            self.Distinct(new FuncEqualityComparer<T>(
                (lhs, rhs) => map(lhs).Equals(map(rhs)),
                item => map(item).GetHashCode()
            ));

        public static IEnumerable<int> Enumerate(this int self) =>
            Enumerable.Range(0, self);

        public static IEnumerable<(T Value, int Index)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((o, i) => (o, i));

        public static IEnumerable<T> Extend<T, U>(this IEnumerable<T> self, IEnumerable<U> other, T with = default)
        {
            using (var selfEnumerator = self.GetEnumerator())
            {
                using (var otherEnumerator = other.GetEnumerator())
                {
                    var selfDidMove = selfEnumerator.MoveNext();
                    var otherDidMove = otherEnumerator.MoveNext();
                    while (otherDidMove || selfDidMove)
                    {
                        if (selfDidMove)
                        {
                            yield return selfEnumerator.Current;
                        }
                        else
                        {
                            yield return with;
                        }

                        selfDidMove = selfEnumerator.MoveNext();
                        otherDidMove = otherEnumerator.MoveNext();
                    }
                }
            }
        }

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
