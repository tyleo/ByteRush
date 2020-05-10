using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph.Definitions
{
    public sealed class GetDef : SimpleDef<GetMeta>
    {
        private static FullName StaticFullName { get; } = FullName.FromLibEnd("System", "Get");
        public static NodeDeclId Id => StaticFullName.NodeDeclId();
        public override FullName FullName => StaticFullName;

        private GetDef() : base(
            Util.NewArray<PortDecl>(),
            Util.NewArray(PortDecl.New("", TypeKind.I32))
        )
        { }

        public static GetDef New() => new GetDef();

        protected override GetMeta DefaultMetaTyped() => GetMeta.New();

        public override void GenerateCode(
            NodeId nodeId,
            in Node node,
            CodeGenState state
        ) => state.SetOutputSymbol(
            nodeId.OutputPortKey(OutputPortId.New(0)),
            VariableSymbol<MI32>.New(node.Meta<GetMeta>().VariableName)
        );
    }
}
