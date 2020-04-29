using System.Runtime.InteropServices;

namespace ByteRush.Util
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Variant
    {
        [FieldOffset(0)]
        public float _float;
        [FieldOffset(0)]
        public int _int;
        [FieldOffset(0)]
        public bool _bool;

        public Variant(float value) : this() => _float = value;
        public Variant(int value) : this() => _int = value;
        public Variant(bool value) : this() => _bool = value;

        public static implicit operator Variant(float value) => new Variant(value);
        public static implicit operator Variant(int value) => new Variant(value);
        public static implicit operator Variant(bool value) => new Variant(value);
    }
}
