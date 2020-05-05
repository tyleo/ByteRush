using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class SetDef : OpDef
    {
        public override string Name => "Set";

        private SetDef() : base(
            Util.NewArray(PortDecl.New("", TypeKind.Exec), PortDecl.New("value", TypeKind.I32)),
            Util.NewArray(PortDecl.New("", TypeKind.Exec))
        )
        { }

        public static SetDef New() => new SetDef();

        public override void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeOnlyState state
        )
        {
            var valueSym = state.GenerateDataBack<MI32>(in node, PortId.New(1));

            valueSym.Release();

            var (_, from, to) = state.OpWriter.Copy();

            state.QueueSymbolAddressWrite(valueSym, from.ToI32());
            state.QueueSymbolAddressWrite(
                VariableSymbol<MI32>.New(node.Meta<SetMeta>().VariableName),
                to.ToI32()
            );

            state.GenerateExecForwards(in node, PortId.New(0));
        }
    }
}
