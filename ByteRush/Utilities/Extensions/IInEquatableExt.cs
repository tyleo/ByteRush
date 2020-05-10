using ByteRush.Utilities.Interface;

namespace ByteRush.Utilities.Extensions
{
    public static class IInEquatableExt
    {
        public static bool InEquatableEquals<T>(in T self, object obj) where T : IInEquatable<T> => obj is T typed && self.Equals(in typed);
    }
}