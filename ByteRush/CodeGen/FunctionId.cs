using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public struct FunctionId : IEquatable<FunctionId>
    {
        private FunctionId(ushort value) => Int = value;

        public static FunctionId New(ushort value) => new FunctionId(value);

        public bool Equals(FunctionId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public static bool operator ==(FunctionId lhs, FunctionId rhs) => Equals(lhs, rhs);

        public static bool operator !=(FunctionId lhs, FunctionId rhs) => !(lhs == rhs);

        public override int GetHashCode() => Int;

        internal ushort Int { get; }
    }
}
