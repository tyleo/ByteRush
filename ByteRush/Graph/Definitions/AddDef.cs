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
            CodeGen.CodeOnlyState state
        )
        {
            var lhsSym = state.GenerateDataBack(in node, PortId.New(0));
            var rhsSym = state.GenerateDataBack(in node, PortId.New(1));

            lhsSym.Release();
            rhsSym.Release();
            var returnSym = state.RetainAnonomyous();

            var (_, from, to, @return) = state.OpWriter.AddI32();
            // TODO: Need to relate these InstructionStreamIndexes to Symbols and store
            // InstructionStreamWriters in _allSymbols. Then when creating the instruction stream
            // is done the resolved symbols can be written back into the instruction stream.

            state.SetOutputSymbol(OutputPortKey.New(nodeId, PortId.New(0)), returnSym);
        }
    }
}
