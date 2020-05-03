using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct OutputPortKey : IEquatable<OutputPortKey>
    {
        private readonly NodeId _nodeId;
        private readonly PortId _portId;

        private OutputPortKey(NodeId nodeId, PortId portId)
        {
            _nodeId = nodeId;
            _portId = portId;
        }

        public static OutputPortKey New(NodeId nodeId, PortId portId) =>
            new OutputPortKey(nodeId, portId);

        public bool Equals(OutputPortKey other) =>
            _nodeId.Equals(other._nodeId) &&
            _portId.Equals(other._portId);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            _nodeId.GetHashCode() ^
            _portId.GetHashCode();
    }
}
