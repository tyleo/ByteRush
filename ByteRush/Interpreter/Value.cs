using ByteRush.Util;

namespace ByteRush.Interpreter
{
    public struct Value
    {
        public Variant _variant;
        public object _reference;

        public T CastRef<T>() => (T)_reference;

        private Value(Variant value, object reference)
        {
            _variant = value;
            _reference = reference;
        }

        public static Value Float(float value) => new Value(new Variant(value), null);
        public static Value Int(int value) => new Value(new Variant(value), null);
        public static Value Bool(bool value) => new Value(new Variant(value), null);
    }
}
