using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeDeclId : IEquatable<NodeDeclId>
    {
        private NodeDeclId(int value) => Int = value;

        public static NodeDeclId New(int value) => new NodeDeclId(value);

        public bool Equals(NodeDeclId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        internal int Int { get; }
    }
}
