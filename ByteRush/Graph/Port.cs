using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph
{
    public struct Port
    {
        private ArrayList<EdgeId> _edges;
        public TypeKind Type { get; }

        public int EdgeCount => _edges.Count;
        public ref EdgeId GetEdge(EdgeId id) => ref _edges[id.Int];
    }
}
