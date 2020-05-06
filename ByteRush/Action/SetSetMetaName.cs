using ByteRush.Graph;

namespace ByteRush.Action
{
    public sealed class SetSetMetaName : IAction
    {
        public ActionKind Kind => ActionKind.SetSetMetaName;

        private readonly NodeKey _node;
        public ref readonly NodeKey Node => ref _node;
        public string Name { get; }

        private SetSetMetaName(in NodeKey node, string name)
        {
            _node = node;
            Name = name;
        }

        public static SetSetMetaName New(in NodeKey node, string name) =>
            new SetSetMetaName(in node, name);
    }
}
