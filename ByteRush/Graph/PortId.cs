using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct PortId : IEquatable<PortId>
    {
        private PortId(int value) => Int = value;

        public static PortId New(int value) => new PortId(value);

        public bool Equals(PortId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        internal int Int { get; }
    }
}
