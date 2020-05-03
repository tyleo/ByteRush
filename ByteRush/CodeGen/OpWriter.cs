using ByteRush.Utilities;
using ByteRush.Interpreter;

namespace ByteRush.CodeGen
{
    public struct OpWriter
    {
        private readonly ArrayList<byte> _bytes;

        public byte[] GetBytes() => _bytes.ToArray();

        public int GetAddress() => _bytes.Count;

        public void Bool(bool value)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(bool));
            ByteUtil.WriteBool(_bytes.Inner, startIndex, value);
        }

        public void U8(byte value)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(byte));
            ByteUtil.WriteU8(_bytes.Inner, startIndex, value);
        }

        public void F32(float value)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(float));
            ByteUtil.WriteF32(_bytes.Inner, startIndex, value);
        }

        public void I32(int value)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteI32(_bytes.Inner, startIndex, value);
        }

        private void Value(Variant value)
        {
            var startIndex = _bytes.Count;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteI32(_bytes.Inner, startIndex, value._int);
        }

        private void Null() => _bytes.Add(0xFF);

        private void Nulls(int amount)
        {
            for (var i = 0; i < amount; ++i) Null();
        }

        public void OpAddInt() => _bytes.Add(Op.AddInt.U8());

        public void OpAddIntStack() => _bytes.Add(Op.AddIntStack.U8());

        public void OpAddIntReg(int lhsOffset, int rhsOffset, int storeOffset)
        {
            _bytes.Add(Op.AddIntReg.U8());
            I32(lhsOffset);
            I32(rhsOffset);
            I32(storeOffset);
        }

        public void OpCopy(int fromOffset, int toOffset)
        {
            _bytes.Add(Op.Copy.U8());
            I32(fromOffset);
            I32(toOffset);
        }

        public void OpGet(byte offset)
        {
            _bytes.Add(Op.Get.U8());
            _bytes.Add(offset);
        }

        public void OpGoto(int address)
        {
            _bytes.Add(Op.Goto.U8());
            I32(address);
        }

        public void OpIncIntStack() => _bytes.Add(Op.IncIntStack.U8());

        public void OpIncIntReg(int offset)
        {
            _bytes.Add(Op.IncIntReg.U8());
            I32(offset);
        }

        public IntInserter OpJumpIfFalse()
        {
            _bytes.Add(Op.JumpIfFalse.U8());
            var insertAddress = GetAddress();
            I32(0);
            return new IntInserter(_bytes, insertAddress);
        }

        public void OpLessThanStack() => _bytes.Add(Op.LessThanStack.U8());

        public void OpLessThanRegPush(int lhsAddress, int rhsAddress)
        {
            _bytes.Add(Op.LessThanRegPush.U8());
            I32(lhsAddress);
            I32(rhsAddress);
        }

        public void OpPushInt(int value)
        {
            _bytes.Add(Op.PushInt.U8());
            I32(value);
        }

        public void OpPushBlock(params Variant[] values)
        {
            _bytes.Add(Op.PushBlock.U8());
            I32(values.Length);
            foreach (var value in values) Value(value);
        }

        public void OpSet(byte offset)
        {
            _bytes.Add(Op.Set.U8());
            _bytes.Add(offset);
        }

        private void Zero() => _bytes.Add(0);

        private void Zeroes(int amount)
        {
            for (var i = 0; i < amount; ++i) Zero();
        }

        private OpWriter(ArrayList<byte> bytes) => _bytes = bytes;

        public static OpWriter New() => new OpWriter(ArrayList<byte>.New());
    }
}
