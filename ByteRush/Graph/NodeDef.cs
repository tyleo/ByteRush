using ByteRush.Utilities;
using System.Collections.Generic;

namespace ByteRush.Graph
{
    public sealed class NodeDef : INodeDecl
    {
        public string Name { get; set; }

        private readonly List<PortDecl> _inputs = new List<PortDecl>();
        public IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef) => _inputs;
        private readonly List<PortDecl> _outputs = new List<PortDecl>();
        public IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef) => _outputs;

        private readonly CompactedList<Node> _nodes = CompactedList<Node>.New();
        private readonly CompactedList<Edge> _edges = CompactedList<Edge>.New();

        public ref readonly Edge GetEdge(EdgeId edgeId) => ref _edges[edgeId.Int];
        public ref readonly Node GetNode(NodeId nodeId) => ref _nodes[nodeId.Int];

        public void AddNode(in Node node) => _nodes.Add(node);

        public void AddEdge(in OutputPortKey from, in InputPortKey to)
        {
            var edge = Edge.New(in from, in to);
            var edgeId = EdgeId.New(_edges.Add(edge));
            GetNode(from.Node).AddOutput(from.Port, edgeId);
            GetNode(to.Node).AddInput(to.Port, edgeId);
        }

        public object DefaultMeta() => null;

        public void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.CodeOnlyState state
        )
        {
            throw new System.NotImplementedException();
        }

        private NodeDef() { }
        public static NodeDef New() => new NodeDef();
    }
}
