using System.Collections.Generic;

namespace ByteRush.Utilities.Extensions
{
    public static class ICollectionExt
    {
        public static void RemoveRange<T>(this ICollection<T> self, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                self.Remove(item);
            }
        }
    }
}
