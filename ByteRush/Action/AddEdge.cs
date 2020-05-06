using ByteRush.Graph;

namespace ByteRush.Action
{
    public sealed class AddEdge : IAction
    {
        public ActionKind Kind => ActionKind.AddEdge;

        public NodeDeclId NodeDef { get; }
        private readonly OutputPortKey _from;
        public ref readonly OutputPortKey From => ref _from;
        private readonly InputPortKey _to;
        public ref readonly InputPortKey To => ref _to;

        private AddEdge(
            NodeDeclId nodeDef,
            in OutputPortKey from,
            in InputPortKey to
        )
        {
            NodeDef = nodeDef;
            _from = from;
            _to = to;
        }

        public static AddEdge New(
            NodeDeclId nodeDef,
            in OutputPortKey from,
            in InputPortKey to
        ) => new AddEdge(nodeDef, in from, in to);
    }
}
