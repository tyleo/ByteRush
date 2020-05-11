using ByteRush.Utilities;
using ByteRush.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.CodeGen
{
    public struct OpCodeWriter
    {
        private readonly ArrayList<byte> _bytes;

        public byte[] GetOpCode() => _bytes.ToArray();

        public OpCodeOnlyAddress<MFinalOpCodeAddress<T>> OpCodeAddress<T>(FinalOpCodeAddress<T>? value = null) => I32(value?.Int ?? -1).Mark<MFinalOpCodeAddress<T>>();

        public OpCodeOnlyAddress<MStackAddress<T>> StackAddress<T>(StackAddress<T>? value = null) => I32(value?.Int ?? -1).Mark<MStackAddress<T>>();

        public OpCodeOnlyAddress<MIntrinsic> Intrinsic(ushort intrinsic = ushort.MaxValue) =>
            U16(intrinsic).Mark<MIntrinsic>();

        public OpCodeOnlyAddress<MI32> I32(int value = -1)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteI32(_bytes.Inner, startIndex, value);
            return OpCodeOnlyAddress<MI32>.New(startIndex);
        }

        public OpCodeOnlyAddress<MOpCode> Op(Op value) => U8(value.U8()).Mark<MOpCode>();

        public OpCodeOnlyAddress<MU16> U16(ushort value = ushort.MaxValue)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(ushort));
            ByteUtil.WriteU16(_bytes.Inner, startIndex, value);
            return OpCodeOnlyAddress<MU16>.New(startIndex);
        }

        public OpCodeOnlyAddress<MU8> U8(byte value = 0xFF)
        {
            var startIndex = _bytes.Count;
            _bytes.Add(value);
            return OpCodeOnlyAddress<MU8>.New(startIndex);
        }

        public PreambleAddress<MValue> Value(Value? value = null)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteI32(_bytes.Inner, startIndex, value?._i32 ?? 0);
            return PreambleAddress<MValue>.New(startIndex);
        }

        public PreambleAddress<MValue>[] Values(IEnumerable<Value?> values)
        {
            var self = this;
            return values.Select(i => self.Value(i)).ToArray();
        }
        public int GetAddress() => _bytes.Count;

        public void WriteI32(int from, FinalOpCodeAddress<MI32> to) =>
            ByteUtil.WriteI32(_bytes.Inner, to.Int, from);

        public void WriteIntrinsic(IntrinsicId from, OpCodeOnlyAddress<MIntrinsic> to) =>
            ByteUtil.WriteU16(_bytes.Inner, to.Int, from.Int);

        public void WriteU8(byte from, OpCodeOnlyAddress<MU8> to) =>
            ByteUtil.WriteU8(_bytes.Inner, to.Int, from);

        public void WriteAddress<T>(FinalOpCodeAddress<T> from, FinalOpCodeAddress<MFinalOpCodeAddress<T>> to) =>
            WriteI32(from.Int, to.Mark<MI32>());

        public void WriteAddress<T>(StackAddress<T> from, FinalOpCodeAddress<MStackAddress<T>> to) =>
            WriteI32(from.Int + 1, to.Mark<MI32>());

        private OpCodeWriter(ArrayList<byte> bytes) => _bytes = bytes;

        public static OpCodeWriter New() => new OpCodeWriter(ArrayList<byte>.New());

        public static OpCodeWriter From(byte[] value) => new OpCodeWriter(ArrayList<byte>.FromArray(value));
    }
}
