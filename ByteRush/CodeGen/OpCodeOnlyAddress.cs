using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public struct OpCodeOnlyAddress<T> : IEquatable<OpCodeOnlyAddress<T>>
    {
        private OpCodeOnlyAddress(int value) => Int = value;

        public static OpCodeOnlyAddress<T> New(int value) => new OpCodeOnlyAddress<T>(value);

        public OpCodeOnlyAddress<U> Mark<U>() => OpCodeOnlyAddress<U>.New(Int);

        public bool Equals(OpCodeOnlyAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class OpCodeOnlyAddress
    {
        public static OpCodeOnlyAddress<T> New<T>(int value) => OpCodeOnlyAddress<T>.New(value);
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MBool>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MF32>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MI32>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MU8>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MBool>> ToBool(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MBool>>();
        public static OpCodeOnlyAddress<MStackAddress<MF32>> ToF32(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MF32>>();
        public static OpCodeOnlyAddress<MStackAddress<MI32>> ToI32(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MI32>>();
        public static OpCodeOnlyAddress<MStackAddress<MU8>> ToU8(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MU8>>();
    }
}
