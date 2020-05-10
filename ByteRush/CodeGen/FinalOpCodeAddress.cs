using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public struct FinalOpCodeAddress<T> : IEquatable<FinalOpCodeAddress<T>>
    {
        private FinalOpCodeAddress(int value) => Int = value;

        public static FinalOpCodeAddress<T> New(int value) => new FinalOpCodeAddress<T>(value);

        public FinalOpCodeAddress<U> Mark<U>() => FinalOpCodeAddress<U>.New(Int);

        public static FinalOpCodeAddress<T> From(
            PreambleAddress<MOpCode> preambleEndAddress,
            OpCodeOnlyAddress<T> opCodeOnlyAddress
        ) => New(preambleEndAddress.Int + opCodeOnlyAddress.Int);

        public bool Equals(FinalOpCodeAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class FinalOpCodeAddress
    {
        public static FinalOpCodeAddress<MOpCode> EndOfProgram => New<MOpCode>(int.MaxValue);
        public static FinalOpCodeAddress<T> From<T>(
            PreambleAddress<MOpCode> preambleEndAddress,
            OpCodeOnlyAddress<T> opCodeOnlyAddress
        ) => FinalOpCodeAddress<T>.From(
            preambleEndAddress,
            opCodeOnlyAddress
        );
        public static FinalOpCodeAddress<T> New<T>(int value) => FinalOpCodeAddress<T>.New(value);
    }
}
