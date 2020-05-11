using ByteRush.Utilities;
using System.Collections.Generic;

namespace ByteRush.Graph
{
    public sealed class FunctionDef : INodeDecl
    {
        public FullName FullName { get; }

        private readonly List<PortDecl> _inputs = new List<PortDecl>();
        public IReadOnlyList<PortDecl> GetInputs(FunctionDef function) => _inputs;
        private readonly List<PortDecl> _outputs = new List<PortDecl>();
        public IReadOnlyList<PortDecl> GetOutputs(FunctionDef function) => _outputs;

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
            CodeGen.CodeGenState state
        )
        {
            throw new System.NotImplementedException();
        }

        private FunctionDef(FullName fullName) => FullName = fullName;
        public static FunctionDef New(FullName fullName) => new FunctionDef(fullName);
    }
}
