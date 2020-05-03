using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class AddDef : OpDef
    {
        public override string Name => "If";

        private AddDef() : base(
            Util.NewArray(PortDecl.New("lhs", TypeKind.I32), PortDecl.New("lhs", TypeKind.I32)),
            Util.NewArray(PortDecl.New("sum", TypeKind.I32))
        ) { }

        public static AddDef New() => new AddDef();

        public override void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGen.State state
        )
        {
            var lhsSym = state.GenerateDataBack(in node, PortId.New(0));
            var rhsSym = state.GenerateDataBack(in node, PortId.New(0));
            // state._opWriter.
        }
    }
}
