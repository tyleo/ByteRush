using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeDeclId : IEquatable<NodeDeclId>
    {
        private readonly Guid _inner;

        private NodeDeclId(Guid inner) => _inner = inner;

        public static NodeDeclId New(Guid inner) => new NodeDeclId(inner);

        public bool Equals(NodeDeclId other) => _inner.Equals(other._inner);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public static bool operator ==(NodeDeclId lhs, NodeDeclId rhs) => lhs.Equals(rhs);

        public static bool operator !=(NodeDeclId lhs, NodeDeclId rhs) => !(lhs == rhs);

        public override int GetHashCode() => _inner.GetHashCode();
    }
}
