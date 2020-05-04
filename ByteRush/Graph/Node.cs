using ByteRush.CodeGen;
using ByteRush.Utilities;

namespace ByteRush.Graph
{
    public struct Node
    {
        public NodeDeclId DeclId { get; }
        public string FullName { get; set; }
        private ArrayList<Value> _defaultValues;

        private ArrayList<Port> _inputs;
        public ref readonly Port GetInput(PortId id) => ref _inputs[id.Int];

        private ArrayList<Port> _outputs;
        public ref readonly Port GetOutput(PortId id) => ref _outputs[id.Int];

        // Stuff like num ports in an array node goes in here
        public object Meta { get; set; }

        public void GenerateCode(
            NodeId nodeId,
            CodeGen.CodeOnlyState state
        ) => state.GraphState.GetNodeDecl(DeclId).GenerateCode(nodeId, in this, state);
    }
}
