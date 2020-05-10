using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public struct StackAddress<T> : IEquatable<StackAddress<T>>
    {
        private StackAddress(int value) => Int = value;

        public static StackAddress<T> New(int value) => new StackAddress<T>(value);

        public StackAddress<U> Mark<U>() => StackAddress<U>.New(Int);

        public bool Equals(StackAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class StackAddress
    {
        public static StackAddress<T> New<T>(int value) => StackAddress<T>.New(value);
    }
}
