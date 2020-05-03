using ByteRush.Utilities.Interface;
using System.Collections.Generic;

namespace ByteRush.Utilities.Extensions
{
    public static class IRefListExt
    {
        public static void AddRange<T>(this IRefList<T> self, IEnumerable<T> values)
        {
            foreach (var value in values) self.Add(in value);
        }

        public static void AddRange<T>(this IRefList<T> self, IRefEnumerable<T> values)
        {
            foreach (var value in values) self.Add(in value);
        }
    }
}
