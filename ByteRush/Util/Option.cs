namespace ByteRush.Util
{
    public struct Option<T>
    {
        private readonly T _value;
        private readonly OptionKind _kind;

        private Option(T value, OptionKind kind)
        {
            _value = value;
            _kind = kind;
        }

        public static Option<T> Some(T value) => new Option<T>(value, OptionKind.Some);
        public static Option<T> None() => new Option<T>(default, OptionKind.None);

        public bool IsSome => _kind == OptionKind.Some;
        public bool IsNone => _kind == OptionKind.None;
        public T Unwrap() => _value;
    }
}
