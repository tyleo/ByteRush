using ByteRush.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities.Extensions
{
    public static class IEnumerableExt
    {
        public static IEnumerable<T> Default<T>(int length)
        {
            for (var i = 0; i < length; ++i) yield return default;
        }

        public static IEnumerable<int> Enumerate(this int self) =>
            Enumerable.Range(0, self);

        public static IEnumerable<(T Value, int Index)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((o, i) => (o, i));

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> op)
        {
            foreach (var i in self) op(i);
        }

        public static IRefEnumerable<T> IntoRef<T>(this IEnumerable<T> self) =>
            RefEnumerableAdapter<T>.New(self);

        public static IEnumerable<(T, U)> Zip<T, U>(IEnumerable<T> lhs, IEnumerable<U> rhs) =>
            lhs.Zip(rhs, (lItem, rItem) => (lItem, rItem));
    }
}
