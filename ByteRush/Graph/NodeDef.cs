using ByteRush.Utilities;
using System.Collections.Generic;

namespace ByteRush.Graph
{
    public sealed class NodeDef : INodeDecl
    {
        public string Name { get; set; }

        private List<PortDecl> _inputs;
        public IReadOnlyList<PortDecl> GetInputs(NodeDef nodeDef) => _inputs;
        private List<PortDecl> _outputs;
        public IReadOnlyList<PortDecl> GetOutputs(NodeDef nodeDef) => _outputs;

        private ArrayList<Node> _nodes;
        private ArrayList<Edge> _edges;

        public ref readonly Edge GetEdge(EdgeId edgeId) => ref _edges[edgeId.Int];
        public ref readonly Node GetNode(NodeId nodeId) => ref _nodes[nodeId.Int];

        public void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.State state
        )
        {
            throw new System.NotImplementedException();
        }
    }
}
