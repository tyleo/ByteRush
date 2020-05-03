using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeDeclId : IEquatable<NodeDeclId>
    {
        private readonly int _value;

        private NodeDeclId(int value) => _value = value;

        public static NodeDeclId New(int value) => new NodeDeclId(value);

        public bool Equals(NodeDeclId other) => _value == other._value;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => _value;

        internal int Int => _value;
    }
}
