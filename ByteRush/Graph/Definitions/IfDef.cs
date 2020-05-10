using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class IfDef : SimpleDef
    {
        private static FullName StaticFullName { get; } = FullName.FromLibEnd("System", "If");
        public static NodeDeclId Id => StaticFullName.NodeDeclId();
        public override FullName FullName => StaticFullName;

        private IfDef() : base(
            Util.NewArray(PortDecl.New("", TypeKind.Exec), PortDecl.New("if", TypeKind.Bool)),
            Util.NewArray(PortDecl.New("than", TypeKind.Exec), PortDecl.New("else", TypeKind.Exec))
        ) { }

        public static IfDef New() => new IfDef();

        public override void GenerateCode(NodeId nodeId, in Node node, CodeGenState state)
        {
            var conditionSym = state.GenerateDataBack<MBool>(in node, InputPortId.New(1));

            conditionSym.Release();

            var (_, condition, jumpAddress) = state.OpWriter.JumpIfFalse();

            state.GenerateExecForwards(in node, OutputPortId.New(0));

            var (_, gotoAddress) = state.OpWriter.Goto();

            var jumpTo = state.OpWriter.GetAddress();

            var next = state.GenerateExecForwards(in node, OutputPortId.New(1));

            var end = state.OpWriter.GetAddress();

            state.QueueSymbolAddressWrite(conditionSym, condition);
            state.QueueOpCodeAddressWrite(jumpTo, jumpAddress);
            state.QueueOpCodeAddressWrite(end, gotoAddress);

            next();
        }
    }
}
