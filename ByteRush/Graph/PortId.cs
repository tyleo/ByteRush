using ByteRush.Util.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct PortId : IEquatable<PortId>
    {
        private readonly int _value;

        private PortId(int value) => _value = value;

        public static PortId New(int value) => new PortId(value);

        public bool Equals(PortId other) => _value == other._value;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => _value;

        internal int Int => _value;
    }
}
