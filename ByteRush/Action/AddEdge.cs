using ByteRush.Graph;

namespace ByteRush.Action
{
    public sealed class AddEdge : IAction
    {
        public ActionKind Kind => ActionKind.AddEdge;

        public NodeDeclId Function { get; }
        private readonly OutputPortKey _from;
        public ref readonly OutputPortKey From => ref _from;
        private readonly InputPortKey _to;
        public ref readonly InputPortKey To => ref _to;

        private AddEdge(
            NodeDeclId function,
            in OutputPortKey from,
            in InputPortKey to
        )
        {
            Function = function;
            _from = from;
            _to = to;
        }

        public static AddEdge New(
            NodeDeclId function,
            in OutputPortKey from,
            in InputPortKey to
        ) => new AddEdge(function, in from, in to);
    }
}
