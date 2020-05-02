using System;

namespace ByteRush.Util.Extensions
{
    public static class EquatableExt
    {
        public static bool EquatableEquals<T>(this T self, object obj) where T : IEquatable<T> => obj is T typed && self.Equals(typed);
    }
}