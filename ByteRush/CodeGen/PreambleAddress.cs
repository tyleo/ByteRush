using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public struct PreambleAddress<T> : IEquatable<PreambleAddress<T>>
    {
        private PreambleAddress(int value) => Int = value;

        public static PreambleAddress<T> New(int value) => new PreambleAddress<T>(value);

        public bool Equals(PreambleAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class PreambleAddress
    {
        public static PreambleAddress<T> New<T>(int value) => PreambleAddress<T>.New(value);
        public static PreambleAddress<MOpCode> Empty() => PreambleAddress<MOpCode>.New(0);
    }
}
