using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct InputPortKey : IEquatable<InputPortKey>
    {
        private readonly NodeId _nodeId;
        private readonly PortId _portId;

        private InputPortKey(NodeId nodeId, PortId portId)
        {
            _nodeId = nodeId;
            _portId = portId;
        }

        public static InputPortKey New(NodeId nodeId, PortId portId) =>
            new InputPortKey(nodeId, portId);

        public bool Equals(InputPortKey other) =>
            _nodeId.Equals(other._nodeId) &&
            _portId.Equals(other._portId);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            _nodeId.GetHashCode() ^
            _portId.GetHashCode();
    }
}
