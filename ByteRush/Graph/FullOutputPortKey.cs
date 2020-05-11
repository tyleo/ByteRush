using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct FullOutputPortKey : IEquatable<FullOutputPortKey>
    {
        public NodeDeclId Function { get; }
        public NodeId Node { get; }
        public OutputPortId Port { get; }

        private FullOutputPortKey(NodeDeclId function, NodeId nodeId, OutputPortId portId)
        {
            Function = function;
            Node = nodeId;
            Port = portId;
        }

        public static FullOutputPortKey New(NodeDeclId function, NodeId nodeId, OutputPortId portId) =>
            new FullOutputPortKey(function, nodeId, portId);

        public bool Equals(FullOutputPortKey other) =>
            Function.Equals(other.Function) &&
            Node.Equals(other.Node) &&
            Port.Equals(other.Port);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            Function.GetHashCode() ^
            Node.GetHashCode() ^
            Port.GetHashCode();
    }
}
