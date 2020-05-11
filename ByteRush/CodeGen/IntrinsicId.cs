using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public struct IntrinsicId : IEquatable<IntrinsicId>
    {
        private IntrinsicId(ushort value) => Int = value;

        public static IntrinsicId New(ushort value) => new IntrinsicId(value);

        public bool Equals(IntrinsicId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public static bool operator ==(IntrinsicId lhs, IntrinsicId rhs) => Equals(lhs, rhs);

        public static bool operator !=(IntrinsicId lhs, IntrinsicId rhs) => !(lhs == rhs);

        public override int GetHashCode() => Int;

        internal ushort Int { get; }
    }
}
