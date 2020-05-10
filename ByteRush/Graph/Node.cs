using ByteRush.CodeGen;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System.Linq;

namespace ByteRush.Graph
{
    public struct Node
    {
        public NodeDeclId DeclId { get; }

        private readonly ArrayList<Value> _defaultValues;
        public ref readonly Value GetDefaultValue(InputPortId id) => ref _defaultValues[id.Int];
        public void SetDefaultValue(InputPortId id, Value value) => _defaultValues[id.Int] = value;

        private readonly ArrayList<Port> _inputs;
        public ref readonly Port GetInput(InputPortId id) => ref _inputs[id.Int];

        private readonly ArrayList<Port> _outputs;
        public ref readonly Port GetOutput(OutputPortId id) => ref _outputs[id.Int];

        // Stuff like num ports in an array node goes in here
        private readonly object _meta;
        public T Meta<T>() => (T)_meta;

        public void AddInput(InputPortId port, EdgeId edge) => GetInput(port).AddEdge(edge);
        public void AddOutput(OutputPortId port, EdgeId edge) => GetOutput(port).AddEdge(edge);

        private Node(
            State graphState,
            NodeDef encapsulatingNodeDef,
            NodeDeclId declId
        )
        {
            var decl = graphState.GetNodeDecl(declId);
            var inputs = decl.GetInputs(encapsulatingNodeDef);
            var outputs = decl.GetOutputs(encapsulatingNodeDef);

            DeclId = declId;
            _defaultValues = ArrayList.FromArray(inputs.Select(_ => new Value()).ToArray());
            _inputs = ArrayList.FromArray(inputs.Select(i => Port.New(i.Type)).ToArray());
            _outputs = ArrayList.FromArray(outputs.Select(i => Port.New(i.Type)).ToArray());
            _meta = decl.DefaultMeta();
        }

        public static Node New(
            State graphState,
            NodeDef encapsulatingNodeDef,
            NodeDeclId declId
        ) => new Node(graphState, encapsulatingNodeDef, declId);

        public void GenerateCode(
            NodeId nodeId,
            CodeGenState state
        ) => state.GraphState.GetNodeDecl(DeclId).GenerateCode(nodeId, in this, state);
    }
}
