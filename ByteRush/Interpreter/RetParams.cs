using ByteRush.Utilities;
using System.Runtime.CompilerServices;

namespace ByteRush.Interpreter
{
    public struct RetParams
    {
        private readonly byte[] _opCode;
        private readonly Value[] _stack;
        private readonly int _paramBase;
        private readonly int _returnBase;

        public RetParams(byte[] opCode, Value[] stack, int paramBase, int returnBase)
        {
            _opCode = opCode;
            _stack = stack;
            _paramBase = paramBase;
            _returnBase = returnBase;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Value GetParam(int paramIndex) => ref _stack[ByteUtil.ReadI32(_opCode, _paramBase + paramIndex * sizeof(int))];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool BoolParam(int i) => GetParam(i)._bool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float F32Param(int i) => GetParam(i)._f32;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int I32Param(int i) => GetParam(i)._i32;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte U8Param(int i) => GetParam(i)._u8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Value GetReturn(int returnIndex) => ref _stack[ByteUtil.ReadI32(_opCode, _returnBase + returnIndex * sizeof(int))];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BoolReturn(int i, bool value) => GetReturn(i)._bool = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void F32Return(int i, float value) => GetReturn(i)._f32 = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void I32Return(int i, int value) => GetReturn(i)._i32 = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void U8Return(int i, byte value) => GetReturn(i)._u8 = value;
    }
}
