using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct FullOutputPortKey : IEquatable<FullOutputPortKey>
    {
        public NodeDeclId NodeDef { get; }
        public NodeId Node { get; }
        public OutputPortId Port { get; }

        private FullOutputPortKey(NodeDeclId nodeDef, NodeId nodeId, OutputPortId portId)
        {
            NodeDef = nodeDef;
            Node = nodeId;
            Port = portId;
        }

        public static FullOutputPortKey New(NodeDeclId nodeDef, NodeId nodeId, OutputPortId portId) =>
            new FullOutputPortKey(nodeDef, nodeId, portId);

        public bool Equals(FullOutputPortKey other) =>
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
