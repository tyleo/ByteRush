namespace ByteRush.Utilities
{
    public sealed class Box<T>
    {
        public T _value;

        private Box(T value) => _value = value;

        public static Box<T> New(T value) => new Box<T>(value);
    }

    public static class Box
    {
        public static Box<T> New<T>(T value) => Box<T>.New(value);
    }
}
