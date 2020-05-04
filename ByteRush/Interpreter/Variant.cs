using ByteRush.Utilities;

namespace ByteRush.Interpreter
{
    public struct Variant
    {
        public Value _variant;
        public object _reference;

        public T CastRef<T>() => (T)_reference;

        private Variant(Value value, object reference)
        {
            _variant = value;
            _reference = reference;
        }

        public static Variant Float(float value) => new Variant(new Value(value), null);
        public static Variant Int(int value) => new Variant(new Value(value), null);
        public static Variant Bool(bool value) => new Variant(new Value(value), null);
    }
}
