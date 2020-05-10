using ByteRush.Graph;

namespace ByteRush.Action
{
    public sealed class SetGetMetaName : IAction
    {
        public ActionKind Kind => ActionKind.SetGetMetaName;

        private readonly NodeKey _node;
        public ref readonly NodeKey Node => ref _node;
        public string Name { get; }

        private SetGetMetaName(in NodeKey node, string name)
        {
            _node = node;
            Name = name;
        }

        public static SetGetMetaName New(in NodeKey node, string name) =>
            new SetGetMetaName(in node, name);
    }
}
