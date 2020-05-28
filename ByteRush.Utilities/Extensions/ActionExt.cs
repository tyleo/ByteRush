namespace ByteRush.Utilities.Extensions
{
    public static class ActionExt
    {
        private static readonly System.Action _empty = () => { };
        public static System.Action Empty() => _empty;
    }
}
