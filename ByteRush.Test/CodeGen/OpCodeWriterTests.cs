using ByteRush.CodeGen;
using ByteRush.Interpreter;
using ByteRush.Utilities;
using NUnit.Framework;
using System.Linq;

namespace ByteRush.Test.CodeGen
{
    public sealed class OpCodeWriterTests
    {
        [Test]
        public void CtorTest()
        {
            var opCodeWriter = OpCodeWriter.New();
            Assert.IsEmpty(opCodeWriter.GetOpCode());
        }

        [Test]
        public void FromTest()
        {
            var from = Util.NewArray<byte>(1, 2, 3);
            var opCodeWriter = OpCodeWriter.From(from);
            Assert.IsTrue(
                opCodeWriter.GetOpCode().SequenceEqual(from)
            );
        }

        [Test]
        public void OpCodeAddressTest()
        {
            var opCodeWriter = OpCodeWriter.New();

            var address0 = opCodeWriter.OpCodeAddress<MUnknown>();
            Assert.AreEqual(0, address0.Int);

            var address1 = opCodeWriter.OpCodeAddress<MUnknown>();
            Assert.AreEqual(sizeof(int), address1.Int);
        }

        [Test]
        public void StackAddressTest()
        {
            var opCodeWriter = OpCodeWriter.New();

            var address0 = opCodeWriter.StackAddress<MUnknown>();
            Assert.AreEqual(0, address0.Int);

            var address1 = opCodeWriter.StackAddress<MUnknown>();
            Assert.AreEqual(sizeof(int), address1.Int);
        }

        [Test]
        public void I32Test()
        {
            var opCodeWriter = OpCodeWriter.New();

            const int expected = 7531;
            var value = opCodeWriter.I32(expected);
            Assert.AreEqual(0, value.Int);
            Assert.AreEqual(expected, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), value.Int));
            Assert.AreEqual(sizeof(int), opCodeWriter.StackAddress<MUnknown>().Int);
        }

        [Test]
        public void OpTest()
        {
            var opCodeWriter = OpCodeWriter.New();

            var value = opCodeWriter.Op(Op.Copy);
            Assert.AreEqual(0, value.Int);
            Assert.AreEqual(Op.Copy.U8(), ByteUtil.ReadU8(opCodeWriter.GetOpCode(), value.Int));
            Assert.AreEqual(sizeof(Op), opCodeWriter.StackAddress<MUnknown>().Int);
        }

        [Test]
        public void U16Test()
        {
            var opCodeWriter = OpCodeWriter.New();

            const ushort expected = 7531;
            var value = opCodeWriter.U16(expected);
            Assert.AreEqual(0, value.Int);
            Assert.AreEqual(expected, ByteUtil.ReadU16(opCodeWriter.GetOpCode(), value.Int));
            Assert.AreEqual(sizeof(ushort), opCodeWriter.StackAddress<MUnknown>().Int);
        }

        [Test]
        public void U8Test()
        {
            var opCodeWriter = OpCodeWriter.New();

            const byte expected = 125;
            var value = opCodeWriter.U8(expected);
            Assert.AreEqual(0, value.Int);
            Assert.AreEqual(expected, ByteUtil.ReadU8(opCodeWriter.GetOpCode(), value.Int));
            Assert.AreEqual(sizeof(byte), opCodeWriter.StackAddress<MUnknown>().Int);
        }

        [Test]
        public void ValueTest()
        {
            var opCodeWriter = OpCodeWriter.New();

            var expected = new Value(125);
            var value = opCodeWriter.Value(expected);
            Assert.AreEqual(0, value.Int);
            Assert.AreEqual(expected._i32, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), value.Int));
            Assert.AreEqual(expected._f32, ByteUtil.ReadF32(opCodeWriter.GetOpCode(), value.Int));
            Assert.AreEqual(sizeof(int), opCodeWriter.StackAddress<MUnknown>().Int);
        }

        [Test]
        public void ValuesTest()
        {
            var opCodeWriter = OpCodeWriter.New();

            var expected0 = new Value(125);
            var expected1 = new Value(125);
            var values = opCodeWriter.Values(Util.NewArray(expected0, expected1).Cast<Value?>());
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual(0, values[0].Int);
            Assert.AreEqual(sizeof(int), values[1].Int);
            Assert.AreEqual(expected0._i32, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), values[0].Int));
            Assert.AreEqual(expected0._f32, ByteUtil.ReadF32(opCodeWriter.GetOpCode(), values[0].Int));
            Assert.AreEqual(expected1._i32, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), values[1].Int));
            Assert.AreEqual(expected1._f32, ByteUtil.ReadF32(opCodeWriter.GetOpCode(), values[1].Int));
            Assert.AreEqual(sizeof(int) * values.Length, opCodeWriter.StackAddress<MUnknown>().Int);
        }

        [Test]
        public void GetAddressTest()
        {
            var opCodeWriter = OpCodeWriter.New();

            Assert.AreEqual(0, opCodeWriter.GetAddress());

            opCodeWriter.I32(0);
            Assert.AreEqual(sizeof(int), opCodeWriter.GetAddress());
        }

        [Test]
        public void WriteI32Test()
        {
            var opCodeWriter = OpCodeWriter.New();

            var i32 = opCodeWriter.I32(0);
            Assert.AreEqual(0, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), i32.Int));

            var writeAddress = FinalOpCodeAddress.From(PreambleAddress.Empty(), i32);

            opCodeWriter.WriteI32(1, writeAddress);
            Assert.AreEqual(1, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), i32.Int));
        }

        [Test]
        public void WriteAddressTest_FinalOpCode()
        {
            var opCodeWriter = OpCodeWriter.New();

            var opCodeAddress = opCodeWriter.Op(Op.Copy);
            var opCodeWriteToAddress = opCodeWriter.OpCodeAddress<MOpCode>();
            Assert.AreEqual(Op.Copy.U8(), ByteUtil.ReadU8(opCodeWriter.GetOpCode(), opCodeAddress.Int));
            Assert.AreNotEqual(opCodeAddress.Int, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), opCodeWriteToAddress.Int));

            var opCodeAddressFinal = FinalOpCodeAddress.From(PreambleAddress.Empty(), opCodeAddress);
            var opCodeWriteToAddressFinal = FinalOpCodeAddress.From(PreambleAddress.Empty(), opCodeWriteToAddress);

            opCodeWriter.WriteAddress(opCodeAddressFinal, opCodeWriteToAddressFinal);
            Assert.AreEqual(opCodeAddressFinal.Int, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), sizeof(byte)));
            Assert.AreEqual(opCodeAddressFinal.Int, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), opCodeWriteToAddressFinal.Int));
        }

        [Test]
        public void WriteAddressTest_Stack()
        {
            var opCodeWriter = OpCodeWriter.New();

            var stackAddress = opCodeWriter.StackAddress<MValue>();
            Assert.AreEqual(stackAddress.Int, 0);
            Assert.AreNotEqual(0, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), stackAddress.Int));

            var stackAddressFinal = FinalOpCodeAddress.From(PreambleAddress.Empty(), stackAddress);

            opCodeWriter.WriteAddress(StackAddress.New<MValue>(0), stackAddressFinal);
            // This is 1 because stack addresses index from the end of the stack. 1 is added here
            // so that we don't need `var address = sp - calculatedAddress - 1` calculations in the
            // VM.
            Assert.AreEqual(1, ByteUtil.ReadI32(opCodeWriter.GetOpCode(), 0));
        }
    }
}
