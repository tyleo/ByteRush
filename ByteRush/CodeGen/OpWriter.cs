using ByteRush.Util;
using ByteRush.Interpreter;

namespace ByteRush.CodeGen
{
    public struct OpWriter
    {
        private readonly ArrayList<byte> _bytes;

        public byte[] GetBytes() => _bytes.ToArray();

        public int GetAddress() => _bytes.Length;

        private void Bool(bool value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(bool));
            ByteUtil.WriteBool(_bytes.Inner, startIndex, value);
        }

        private void Byte(byte value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(byte));
            ByteUtil.WriteByte(_bytes.Inner, startIndex, value);
        }

        private void Float(float value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(float));
            ByteUtil.WriteFloat(_bytes.Inner, startIndex, value);
        }

        private void Int(int value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteInt(_bytes.Inner, startIndex, value);
        }

        private void Value(Variant value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteInt(_bytes.Inner, startIndex, value._int);
        }

        private void Null() => _bytes.Add(0xFF);

        private void Nulls(int amount)
        {
            for (var i = 0; i < amount; ++i) Null();
        }

        public void OpAddInt() => _bytes.Add(Op.AddIntStack.Byte());

        public void OpAddIntReg(int lhsOffset, int rhsOffset, int storeOffset)
        {
            _bytes.Add(Op.AddIntReg.Byte());
            Int(lhsOffset);
            Int(rhsOffset);
            Int(storeOffset);
        }

        public void OpCopy(int fromOffset, int toOffset)
        {
            _bytes.Add(Op.Copy.Byte());
            Int(fromOffset);
            Int(toOffset);
        }

        public void OpGet(byte offset)
        {
            _bytes.Add(Op.Get.Byte());
            _bytes.Add(offset);
        }

        public void OpGoto(int address)
        {
            _bytes.Add(Op.Goto.Byte());
            Int(address);
        }

        public void OpIncIntStack() => _bytes.Add(Op.IncIntStack.Byte());

        public void OpIncIntReg(int offset)
        {
            _bytes.Add(Op.IncIntReg.Byte());
            Int(offset);
        }

        public IntInserter OpJumpIfFalse()
        {
            _bytes.Add(Op.JumpIfFalse.Byte());
            var insertAddress = GetAddress();
            Int(0);
            return new IntInserter(_bytes, insertAddress);
        }

        public void OpLessThanStack() => _bytes.Add(Op.LessThanStack.Byte());

        public void OpLessThanRegPush(int lhsAddress, int rhsAddress)
        {
            _bytes.Add(Op.LessThanRegPush.Byte());
            Int(lhsAddress);
            Int(rhsAddress);
        }

        public void OpPushInt(int value)
        {
            _bytes.Add(Op.PushInt.Byte());
            Int(value);
        }

        public void OpPushBlock(params Variant[] values)
        {
            _bytes.Add(Op.PushBlock.Byte());
            Int(values.Length);
            foreach (var value in values) Value(value);
        }

        public void OpSet(byte offset)
        {
            _bytes.Add(Op.Set.Byte());
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
