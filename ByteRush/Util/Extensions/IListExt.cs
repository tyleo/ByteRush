using System.Collections.Generic;

namespace ByteRush.Util.Extensions
{
    public static class IListExt
    {
        public static T Last<T>(this IList<T> self) => self[self.Count - 1];

        public static T Pop<T>(this IList<T> self)
        {
            var result = self.Last();
            self.RemoveAt(self.Count - 1);
            return result;
        }
    }
}
