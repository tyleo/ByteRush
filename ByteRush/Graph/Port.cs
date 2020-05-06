using ByteRush.Utilities;

namespace ByteRush.Graph
{
    public struct Port
    {
        private readonly ArrayList<EdgeId> _edges;
        public TypeKind Type { get; }

        public int EdgeCount => _edges.Count;
        public ref EdgeId GetEdge(EdgeId id) => ref _edges[id.Int];
        public void AddEdge(EdgeId id) => _edges.Add(id);

        private Port(TypeKind type)
        {
            _edges = ArrayList.New<EdgeId>();
            Type = type;
        }

        public static Port New(TypeKind type) => new Port(type);
    }
}
