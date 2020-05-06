using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct NodeKey : IEquatable<NodeKey>
    {
        public NodeDeclId NodeDef { get; }
        public NodeId Node { get; }

        private NodeKey(NodeDeclId nodeDef, NodeId node)
        {
            NodeDef = nodeDef;
            Node = node;
        }

        public static NodeKey New(NodeDeclId nodeDef, NodeId node) =>
            new NodeKey(nodeDef, node);

        public bool Equals(NodeKey other) =>
            NodeDef.Equals(other.NodeDef) &&
            Node.Equals(other.Node);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            NodeDef.GetHashCode() ^
            Node.GetHashCode();
    }
}
