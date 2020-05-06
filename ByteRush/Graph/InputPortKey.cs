using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct InputPortKey : IEquatable<InputPortKey>
    {
        public NodeId Node { get; }
        public InputPortId Port { get; }

        private InputPortKey(NodeId nodeId, InputPortId portId)
        {
            Node = nodeId;
            Port = portId;
        }

        public static InputPortKey New(NodeId nodeId, InputPortId portId) =>
            new InputPortKey(nodeId, portId);

        public bool Equals(InputPortKey other) =>
            Node.Equals(other.Node) &&
            Port.Equals(other.Port);

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() =>
            Node.GetHashCode() ^
            Port.GetHashCode();
    }
}
