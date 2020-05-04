using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeId : IEquatable<NodeId>
    {
        private NodeId(int value) => Int = value;

        public static NodeId New(int value) => new NodeId(value);

        public bool Equals(NodeId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }
}
