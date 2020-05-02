using ByteRush.Util.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct EdgeId : IEquatable<EdgeId>
    {
        private readonly int _value;

        private EdgeId(int value) => _value = value;

        public static EdgeId New(int value) => new EdgeId(value);

        public bool Equals(EdgeId other) => _value == other._value;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => _value;

        internal int Int => _value;
    }
}
