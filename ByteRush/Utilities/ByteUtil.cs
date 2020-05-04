using ByteRush.Utilities.Extensions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ByteRush.Utilities
{
    public static class ByteUtil
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct BoolByte
        {
            [FieldOffset(0)]
            public bool _bool;
            [FieldOffset(0)]
            public byte _byte;

            public BoolByte(bool value) : this() => _bool = value;
            public BoolByte(byte value) : this() => _byte = value;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatInt
        {
            [FieldOffset(0)]
            public float _float;
            [FieldOffset(0)]
            public int _int;

            public FloatInt(float value) : this() => _float = value;
            public FloatInt(int value) : this() => _int = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(byte[] bytes, int index) => new BoolByte(bytes[index])._bool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadF32(byte[] bytes, int index)
        {
            var intValue = bytes[index + 0] << 0 |
            bytes[index + 1] << 8 |
            bytes[index + 2] << 16 |
            bytes[index + 3] << 24;
            return new FloatInt(intValue)._float;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadI32(byte[] bytes, int index) =>
            bytes[index + 0] << 0 |
            bytes[index + 1] << 8 |
            bytes[index + 2] << 16 |
            bytes[index + 3] << 24;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadU8(byte[] bytes, int index) => bytes[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadU16(byte[] bytes, int index) =>
            (ushort)(
                bytes[index + 0] << 0 |
                bytes[index + 1] << 8
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBool(byte[] bytes, int index, bool value) => bytes[index] = new BoolByte(value)._byte;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU8(byte[] bytes, int index, byte value) => bytes[index] = value;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteF32(byte[] bytes, int index, float value)
        {
            var intValue = new FloatInt(value)._int;
            bytes[index + 0] = (intValue >> 0).Byte();
            bytes[index + 1] = (intValue >> 8).Byte();
            bytes[index + 2] = (intValue >> 16).Byte();
            bytes[index + 3] = (intValue >> 24).Byte();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteI32(byte[] bytes, int index, int value)
        {
            bytes[index + 0] = (value >> 0).Byte();
            bytes[index + 1] = (value >> 8).Byte();
            bytes[index + 2] = (value >> 16).Byte();
            bytes[index + 3] = (value >> 24).Byte();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU16(byte[] bytes, int index, ushort value)
        {
            bytes[index + 0] = (value >> 0).Byte();
            bytes[index + 1] = (value >> 8).Byte();
        }
    }
}
