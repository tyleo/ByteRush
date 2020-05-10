using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class LessThanDef : SimpleDef
    {
        private static FullName StaticFullName { get; } = FullName.FromLibEnd("System", "LessThan");
        public static NodeDeclId Id => StaticFullName.NodeDeclId();
        public override FullName FullName => StaticFullName;

        private LessThanDef() : base(
            Util.NewArray(PortDecl.New("lhs", TypeKind.I32), PortDecl.New("lhs", TypeKind.I32)),
            Util.NewArray(PortDecl.New("sum", TypeKind.Bool))
        )
        { }

        public static LessThanDef New() => new LessThanDef();

        public override void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGenState state
        )
        {
            var lhsSym = state.GenerateDataBack<MI32>(in node, InputPortId.New(0));
            var rhsSym = state.GenerateDataBack<MI32>(in node, InputPortId.New(1));

            lhsSym.Release();
            rhsSym.Release();

            var (_, lhs, rhs, @return) = state.OpWriter.LessThanI32();

            var returnPortEdges = node.GetOutput(OutputPortId.New(0)).EdgeCount;
            var returnSym = state.RetainAnonomyous<MBool>(returnPortEdges);

            state.QueueSymbolAddressWrite(lhsSym, lhs);
            state.QueueSymbolAddressWrite(rhsSym, rhs);
            state.QueueSymbolAddressWrite(returnSym, @return);

            state.SetOutputSymbol(OutputPortKey.New(nodeId, OutputPortId.New(0)), returnSym);
        }
    }
}
