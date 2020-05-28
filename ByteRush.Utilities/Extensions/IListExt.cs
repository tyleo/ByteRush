using System.Collections.Generic;

namespace ByteRush.Utilities.Extensions
{
    public static class IListExt
    {
        public static T GetOrAdd<T>(this IList<T> self, int i, T value)
        {
            if (i >= self.Count)
            {
                self[i] = value;
            }

            return self[i];
        }

        public static T Last<T>(this IList<T> self) => self[self.Count - 1];

        public static T SetLast<T>(this IList<T> self, T value) => self[self.Count - 1] = value;

        public static T Pop<T>(this IList<T> self)
        {
            var result = self.Last();
            self.RemoveAt(self.Count - 1);
            return result;
        }
    }
}
