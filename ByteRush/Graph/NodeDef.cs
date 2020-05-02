using ByteRush.Util;

namespace ByteRush.Graph
{
    public struct NodeDef
    {
        public string Name { get; set; }

        private ArrayList<PortDecl> _inputs;
        private ArrayList<PortDecl> _outputs;

        private ArrayList<Node> _node;
        private ArrayList<Edge> _edges;
    }
}
