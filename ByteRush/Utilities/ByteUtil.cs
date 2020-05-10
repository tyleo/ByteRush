using ByteRush.Utilities.Extensions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ByteRush.Utilities
{
    public static class ByteUtil
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct BoolU8
        {
            [FieldOffset(0)]
            public bool _bool;
            [FieldOffset(0)]
            public byte _u8;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public BoolU8(bool value) : this() => _bool = value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public BoolU8(byte value) : this() => _u8 = value;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct CharU16
        {
            [FieldOffset(0)]
            public char _char;
            [FieldOffset(0)]
            public ushort _ushort;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public CharU16(char value) : this() => _char = value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public CharU16(ushort value) : this() => _ushort = value;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct F32I32
        {
            [FieldOffset(0)]
            public float _f32;
            [FieldOffset(0)]
            public int _i32;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public F32I32(float value) : this() => _f32 = value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public F32I32(int value) : this() => _i32 = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(byte[] bytes, int index) => new BoolU8(bytes[index])._bool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadChar(byte[] bytes, int index) =>
            new CharU16(ReadU16(bytes, index))._char;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadF32(byte[] bytes, int index)
        {
            var intValue = bytes[index + 0] << 0 |
            bytes[index + 1] << 8 |
            bytes[index + 2] << 16 |
            bytes[index + 3] << 24;
            return new F32I32(intValue)._f32;
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
        public static void WriteBool(byte[] bytes, int index, bool value) => bytes[index] = new BoolU8(value)._u8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteChar(byte[] bytes, int index, char value) =>
            WriteU16(bytes, index, new CharU16(value)._ushort);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteF32(byte[] bytes, int index, float value)
        {
            var intValue = new F32I32(value)._i32;
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
        public static void WriteU8(byte[] bytes, int index, byte value) => bytes[index] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU16(byte[] bytes, int index, ushort value)
        {
            bytes[index + 0] = (value >> 0).Byte();
            bytes[index + 1] = (value >> 8).Byte();
        }
    }
}
