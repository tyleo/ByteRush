using ByteRush.Util;

namespace ByteRush.Graph
{
    public struct NodeDecl
    {
        public CodeGenerator CodeGenerator { get; }

        private readonly PortDecl[] _inputs;
        private readonly PortDecl[] _outputs;

        private NodeDecl(
            CodeGenerator codeGenerator,
            PortDecl[] inputs,
            PortDecl[] outputs
        )
        {
            CodeGenerator = codeGenerator;
            _inputs = inputs;
            _outputs = outputs;
        }

        public static NodeDecl New(
            CodeGenerator codeGenerator,
            PortDecl[] inputs,
            PortDecl[] outputs
        ) => new NodeDecl(codeGenerator, inputs, outputs);
    }
}
