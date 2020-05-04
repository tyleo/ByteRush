using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class IfDef : OpDef
    {
        public override string Name => "If";

        private IfDef() : base(
            Util.NewArray(PortDecl.New("lhs", TypeKind.I32), PortDecl.New("lhs", TypeKind.I32)),
            Util.NewArray(PortDecl.New("sum", TypeKind.I32))
        ) { }

        public static IfDef New() => new IfDef();

        public override void GenerateCode(NodeId nodeId, in Node node, CodeGen.CodeOnlyState state)
        {
            throw new System.NotImplementedException();
        }
    }
}
