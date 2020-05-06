using ByteRush.Graph;
using ByteRush.Utilities;

namespace ByteRush.Action
{
    public sealed class SetDefaultValue : IAction
    {
        public ActionKind Kind => ActionKind.SetDefaultValue;

        private readonly FullInputPortKey _port;
        public ref readonly FullInputPortKey Port => ref _port;

        public Value Value { get; }

        private SetDefaultValue(in FullInputPortKey port, Value value)
        {
            _port = port;
            Value = value;
        }

        public static SetDefaultValue New(in FullInputPortKey port, Value value) =>
            new SetDefaultValue(in port, value);
    }
}
