using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct FullInputPortKey : IEquatable<FullInputPortKey>
    {
        public NodeDeclId NodeDef { get; }
        public NodeId Node { get; }
        public InputPortId Port { get; }

        private FullInputPortKey(NodeDeclId nodeDef, NodeId nodeId, InputPortId portId)
        {
            NodeDef = nodeDef;
            Node = nodeId;
            Port = portId;
        }

        public static FullInputPortKey New(NodeDeclId nodeDef, NodeId nodeId, InputPortId portId) =>
            new FullInputPortKey(nodeDef, nodeId, portId);

        public bool Equals(FullInputPortKey other) =>
            NodeDef.Equals(other.NodeDef) &&
            Node.Equals(other.Node) &&
            Port.Equals(other.Port);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            NodeDef.GetHashCode() ^
            Node.GetHashCode() ^
            Port.GetHashCode();
    }
}
