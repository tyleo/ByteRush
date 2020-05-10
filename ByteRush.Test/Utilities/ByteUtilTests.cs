using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace ByteRush.Test.Utilities
{
    public sealed class ByteUtilTests
    {
        private static byte[] Values<T>(Action<byte[], int, T> write, int size, T[] values)
        {
            var array = (size * values.Length).Enumerate().Select(i => (byte)i).ToArray();
            foreach (var (value, i) in values.Enumerate()) write(array, i * size, value);
            return array;
        }

        [Test]
        public void ReadWriteBoolTest()
        {
            byte[] Bools(params bool[] values) => Values(ByteUtil.WriteBool, sizeof(bool), values);

            var falseBytes = Bools(false);
            var trueBytes = Bools(true);
            var falseBytesOffset = Bools(true, false);
            var trueBytesOffset = Bools(false, true);

            Assert.IsFalse(ByteUtil.ReadBool(falseBytes, 0));
            Assert.IsTrue(ByteUtil.ReadBool(trueBytes, 0));

            Assert.IsTrue(ByteUtil.ReadBool(falseBytesOffset, 0));
            Assert.IsFalse(ByteUtil.ReadBool(falseBytesOffset, 1));

            Assert.IsFalse(ByteUtil.ReadBool(trueBytesOffset, 0));
            Assert.IsTrue(ByteUtil.ReadBool(trueBytesOffset, 1));
        }

        [Test]
        public void ReadWriteF32Test()
        {
            byte[] F32s(params float[] values) => Values(ByteUtil.WriteF32, sizeof(float), values);

            var zeroFloat = F32s(0);
            const float random = 27;
            var randomFloat = F32s(random);
            var maxFloat = F32s(float.MaxValue);
            var nanFloat = F32s(float.NaN);
            var infFloat = F32s(float.PositiveInfinity);
            var negInfFloat = F32s(float.NegativeInfinity);
            var minFloat = F32s(float.MinValue);
            var epsilonFloat = F32s(float.Epsilon);
            var twoFloats = F32s(0, 1);

            Assert.AreEqual(0f, ByteUtil.ReadF32(zeroFloat, 0));
            Assert.AreEqual(random, ByteUtil.ReadF32(randomFloat, 0));
            Assert.AreEqual(float.MaxValue, ByteUtil.ReadF32(maxFloat, 0));
            Assert.AreEqual(float.NaN, ByteUtil.ReadF32(nanFloat, 0));
            Assert.AreEqual(float.PositiveInfinity, ByteUtil.ReadF32(infFloat, 0));
            Assert.AreEqual(float.NegativeInfinity, ByteUtil.ReadF32(negInfFloat, 0));
            Assert.AreEqual(float.MinValue, ByteUtil.ReadF32(minFloat, 0));
            Assert.AreEqual(float.Epsilon, ByteUtil.ReadF32(epsilonFloat, 0));

            Assert.AreEqual(0f, ByteUtil.ReadF32(twoFloats, 0));
            Assert.AreEqual(1f, ByteUtil.ReadF32(twoFloats, sizeof(float)));
        }

        [Test]
        public void ReadWriteI32Test()
        {
            byte[] I32s(params int[] values) => Values(ByteUtil.WriteI32, sizeof(int), values);

            var zero = I32s(0);
            const int randomVal = 27;
            var random = I32s(randomVal);
            var max = I32s(int.MaxValue);
            var min = I32s(int.MinValue);
            var two = I32s(0, 1);

            Assert.AreEqual(0f, ByteUtil.ReadI32(zero, 0));
            Assert.AreEqual(randomVal, ByteUtil.ReadI32(random, 0));
            Assert.AreEqual(int.MaxValue, ByteUtil.ReadI32(max, 0));
            Assert.AreEqual(int.MinValue, ByteUtil.ReadI32(min, 0));

            Assert.AreEqual(0, ByteUtil.ReadI32(two, 0));
            Assert.AreEqual(1, ByteUtil.ReadI32(two, sizeof(int)));
        }

        [Test]
        public void ReadWriteU8Test()
        {
            byte[] U8s(params byte[] values) => Values(ByteUtil.WriteU8, sizeof(byte), values);

            var zero = U8s(0);
            const byte randomVal = 27;
            var random = U8s(randomVal);
            var max = U8s(byte.MaxValue);
            var min = U8s(byte.MinValue);
            var two = U8s(0, 1);

            Assert.AreEqual(0f, ByteUtil.ReadU8(zero, 0));
            Assert.AreEqual(randomVal, ByteUtil.ReadU8(random, 0));
            Assert.AreEqual(byte.MaxValue, ByteUtil.ReadU8(max, 0));
            Assert.AreEqual(byte.MinValue, ByteUtil.ReadU8(min, 0));

            Assert.AreEqual(0, ByteUtil.ReadU8(two, 0));
            Assert.AreEqual(1, ByteUtil.ReadU8(two, sizeof(byte)));
        }

        [Test]
        public void ReadWriteU16Test()
        {
            byte[] U16s(params ushort[] values) => Values(ByteUtil.WriteU16, sizeof(ushort), values);

            var zero = U16s(0);
            const ushort randomVal = 27;
            var random = U16s(randomVal);
            var max = U16s(ushort.MaxValue);
            var min = U16s(ushort.MinValue);
            var two = U16s(0, 1);

            Assert.AreEqual(0f, ByteUtil.ReadU16(zero, 0));
            Assert.AreEqual(randomVal, ByteUtil.ReadU16(random, 0));
            Assert.AreEqual(ushort.MaxValue, ByteUtil.ReadU16(max, 0));
            Assert.AreEqual(ushort.MinValue, ByteUtil.ReadU16(min, 0));

            Assert.AreEqual(0, ByteUtil.ReadU16(two, 0));
            Assert.AreEqual(1, ByteUtil.ReadU16(two, sizeof(ushort)));
        }
    }
}
