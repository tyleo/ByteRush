using ByteRush.Util.Interface;
using System.Collections.Generic;

namespace ByteRush.Util.Extensions
{
    public static class IEnumerableExt
    {
        public static IRefEnumerable<T> IntoRef<T>(this IEnumerable<T> self) =>
            RefEnumerableAdapter<T>.New(self);
    }
}
