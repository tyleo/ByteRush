using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct EdgeId : IEquatable<EdgeId>
    {
        private EdgeId(int value) => Int = value;

        public static EdgeId New(int value) => new EdgeId(value);

        public bool Equals(EdgeId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        internal int Int { get; }
    }
}
