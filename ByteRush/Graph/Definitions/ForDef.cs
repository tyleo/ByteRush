using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class ForDef : SimpleDef
    {
        private static FullName StaticFullName { get; } = FullName.FromLibEnd("System", "For");
        public static NodeDeclId Id => StaticFullName.NodeDeclId();
        public override FullName FullName => StaticFullName;

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

        public override void GenerateCode(NodeId nodeId, in Node node, CodeGenState state)
        {
            var startSym = state.GenerateDataBack<MI32>(in node, InputPortId.New(1));
            startSym.Release();

            var iUsageEdges = node.GetOutput(OutputPortId.New(1)).EdgeCount;
            var iSym = state.RetainAnonomyous<MI32>(iUsageEdges + 1);

            var (_, from, to) = state.OpWriter.Copy();

            var top = state.OpWriter.GetAddress();

            var endSym = state.GenerateDataBack<MI32>(in node, InputPortId.New(2));
            endSym.Release();

            var isLessThanSym = state.RetainAnonomyous<MBool>(1);

            var (_, lhs, rhs, isLessThan) = state.OpWriter.LessThanI32();
            var (_, condition, gotoEnd) = state.OpWriter.JumpIfFalse();

            state.GenerateExecForwards(in node, OutputPortId.New(0));

            var (_, incI) = state.OpWriter.IncI32();
            var (_, gotoStart) = state.OpWriter.Goto();

            iSym.Release();
            isLessThanSym.Release();

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

            state.SetOutputSymbol(OutputPortKey.New(nodeId, OutputPortId.New(1)), iSym);
            state.GenerateExecForwards(in node, OutputPortId.New(2));
        }
    }
}
