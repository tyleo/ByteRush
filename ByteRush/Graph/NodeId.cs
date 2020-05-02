using ByteRush.Util.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeId : IEquatable<NodeId>
    {
        private readonly int _value;

        private NodeId(int value) => _value = value;

        public static NodeId New(int value) => new NodeId(value);

        public bool Equals(NodeId other) => _value == other._value;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => _value;
    }
}
