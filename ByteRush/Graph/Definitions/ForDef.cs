using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class ForDef : OpDef
    {
        public override string Name => "For";

        private ForDef() : base(
            Util.NewArray(
                PortDecl.New("", TypeKind.Exec),
                PortDecl.New("start (inclusive)", TypeKind.I32),
                PortDecl.New("end (exclusive)", TypeKind.I32)
            ),
            Util.NewArray(
                PortDecl.New("loop", TypeKind.Exec),
                PortDecl.New("index", TypeKind.I32),
                PortDecl.New("next", TypeKind.Exec)
            )
        )
        { }

        public static ForDef New() => new ForDef();

        public override void GenerateCode(NodeId nodeId, in Node node, CodeOnlyState state)
        {
            var startSym = state.GenerateDataBack<MI32>(in node, PortId.New(1));
            startSym.Release();

            var iUsageEdges = node.GetOutput(PortId.New(1)).EdgeCount;
            var iSym = state.RetainAnonomyous<MI32>(iUsageEdges + 1);

            var (_, from, to) = state.OpWriter.Copy();

            var top = state.OpWriter.GetAddress();

            var endSym = state.GenerateDataBack<MI32>(in node, PortId.New(2));
            endSym.Release();

            var isLessThanSym = state.RetainAnonomyous<MBool>(0);

            var (_, lhs, rhs, isLessThan) = state.OpWriter.LessThanI32();
            var (_, condition, gotoEnd) = state.OpWriter.JumpIfFalse();

            state.GenerateExecForwards(in node, PortId.New(0));

            var (_, incI) = state.OpWriter.IncI32();
            var (_, gotoStart) = state.OpWriter.Goto();

            iSym.Release();

            var bottom = state.OpWriter.GetAddress();

            state.QueueSymbolAddressWrite(startSym, from.ToI32());
            state.QueueSymbolAddressWrite(iSym, to.ToI32());
            state.QueueSymbolAddressWrite(iSym, lhs);
            state.QueueSymbolAddressWrite(endSym, rhs);
            state.QueueSymbolAddressWrite(isLessThanSym, isLessThan);
            state.QueueSymbolAddressWrite(isLessThanSym, condition);
            state.QueueSymbolAddressWrite(iSym, incI);

            state.QueueOpCodeAddressWrite(top, gotoStart);
            state.QueueOpCodeAddressWrite(bottom, gotoEnd);

            state.SetOutputSymbol(OutputPortKey.New(nodeId, PortId.New(1)), iSym);
            state.GenerateExecForwards(in node, PortId.New(2));
        }
    }
}
