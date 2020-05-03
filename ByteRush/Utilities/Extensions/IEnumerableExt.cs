using ByteRush.Utilities.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Utilities.Extensions
{
    public static class IEnumerableExt
    {
        public static IEnumerable<(T Value, int Index)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((o, i) => (o, i));

        public static IRefEnumerable<T> IntoRef<T>(this IEnumerable<T> self) =>
            RefEnumerableAdapter<T>.New(self);

        public static IEnumerable<(T, U)> Zip<T, U>(IEnumerable<T> lhs, IEnumerable<U> rhs) =>
            lhs.Zip(rhs, (lItem, rItem) => (lItem, rItem));
    }
}
