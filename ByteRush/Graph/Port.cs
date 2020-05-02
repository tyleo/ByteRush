using ByteRush.CodeGen;
using ByteRush.Util;

namespace ByteRush.Graph
{
    public struct Port
    {
        private ArrayList<EdgeId> _edges;
        private TypeKind _type;

        public ref EdgeId GetEdge(EdgeId id) => ref _edges[id.Int];
    }
}
