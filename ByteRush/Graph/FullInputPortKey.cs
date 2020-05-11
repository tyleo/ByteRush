using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct FullInputPortKey : IEquatable<FullInputPortKey>
    {
        public NodeDeclId Function { get; }
        public NodeId Node { get; }
        public InputPortId Port { get; }

        private FullInputPortKey(NodeDeclId function, NodeId nodeId, InputPortId portId)
        {
            Function = function;
            Node = nodeId;
            Port = portId;
        }

        public static FullInputPortKey New(NodeDeclId function, NodeId nodeId, InputPortId portId) =>
            new FullInputPortKey(function, nodeId, portId);

        public bool Equals(FullInputPortKey other) =>
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
