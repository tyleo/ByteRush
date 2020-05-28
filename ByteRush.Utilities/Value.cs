using System.Runtime.InteropServices;

namespace ByteRush.Utilities
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Value
    {
        [FieldOffset(0)]
        public float _f32;
        [FieldOffset(0)]
        public int _i32;
        [FieldOffset(0)]
        public bool _bool;
        [FieldOffset(0)]
        public byte _u8;

        public Value(float value) : this() => _f32 = value;
        public Value(int value) : this() => _i32 = value;
        public Value(bool value) : this() => _bool = value;
        public Value(byte value) : this() => _u8 = value;

        public static Value F32(float value) => new Value(value);
        public static Value I32(int value) => new Value(value);
        public static Value Bool(bool value) => new Value(value);
        public static Value U8(byte value) => new Value(value);

        public static implicit operator Value(float value) => new Value(value);
        public static implicit operator Value(int value) => new Value(value);
        public static implicit operator Value(bool value) => new Value(value);
        public static implicit operator Value(byte value) => new Value(value);

        public override bool Equals(object obj) =>
            obj is Value value && _i32 == value._i32;

        public override int GetHashCode() =>
            1741969168 + _i32.GetHashCode();
    }
}
