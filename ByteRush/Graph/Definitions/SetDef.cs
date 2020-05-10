using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class SetDef : SimpleDef<SetMeta>
    {
        private static FullName StaticFullName { get; } = FullName.FromLibEnd("System", "Set");
        public static NodeDeclId Id => StaticFullName.NodeDeclId();
        public override FullName FullName => StaticFullName;

        private SetDef() : base(
            Util.NewArray(PortDecl.New("", TypeKind.Exec), PortDecl.New("value", TypeKind.I32)),
            Util.NewArray(PortDecl.New("", TypeKind.Exec))
        )
        { }

        public static SetDef New() => new SetDef();

        protected override SetMeta DefaultMetaTyped() => SetMeta.New();

        public override void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGenState state
        )
        {
            var valueSym = state.GenerateDataBack<MI32>(in node, InputPortId.New(1));

            valueSym.Release();

            var (_, from, to) = state.OpWriter.Copy();

            state.QueueSymbolAddressWrite(valueSym, from.ToI32());
            state.QueueSymbolAddressWrite(
                VariableSymbol<MI32>.New(node.Meta<SetMeta>().VariableName),
                to.ToI32()
            );

            state.GenerateExecForwards(in node, OutputPortId.New(0));
        }
    }
}
