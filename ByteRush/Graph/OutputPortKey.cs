using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct OutputPortKey : IEquatable<OutputPortKey>
    {
        public NodeId Node { get; }
        public OutputPortId Port { get; }

        private OutputPortKey(NodeId nodeId, OutputPortId portId)
        {
            Node = nodeId;
            Port = portId;
        }

        public static OutputPortKey New(NodeId nodeId, OutputPortId portId) =>
            new OutputPortKey(nodeId, portId);

        public bool Equals(OutputPortKey other) =>
            Node.Equals(other.Node) &&
            Port.Equals(other.Port);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            Node.GetHashCode() ^
            Port.GetHashCode();
    }
}
