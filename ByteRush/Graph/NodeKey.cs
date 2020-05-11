using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeKey : IEquatable<NodeKey>
    {
        public NodeDeclId Function { get; }
        public NodeId Node { get; }

        private NodeKey(NodeDeclId function, NodeId node)
        {
            Function = function;
            Node = node;
        }

        public static NodeKey New(NodeDeclId function, NodeId node) =>
            new NodeKey(function, node);

        public bool Equals(NodeKey other) =>
            Function.Equals(other.Function) &&
            Node.Equals(other.Node);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            Function.GetHashCode() ^
            Node.GetHashCode();
    }
}
