using ByteRush.Utilities.Interface;
using System;

namespace ByteRush.Utilities.Extensions
{
    public static class IRefEnumerableExt
    {
        public delegate void InAction<T>(in T arg0);

        public static void ForEach<T>(this IRefEnumerable<T> self, InAction<T> op)
        {
            var enumerator = self.GetEnumerator();
            while (enumerator.MoveNext())
            {
                op(in enumerator.Current);
            }
        }
    }
}
