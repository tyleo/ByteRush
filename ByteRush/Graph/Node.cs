using ByteRush.CodeGen;
using ByteRush.Util;

namespace ByteRush.Graph
{
    public struct Node
    {
        public NodeKind Kind { get; set; }
        public int DefOrDeclId { get; }
        public string FullName { get; set; }
        private ArrayList<Variant> _defaultValues;

        private ArrayList<Port> _inputs;
        public ref Port GetInput(PortId id) => ref _inputs[id.Int];

        private ArrayList<Port> _outputs;
        public ref Port GetOutput(PortId id) => ref _outputs[id.Int];

        public void GenerateCode(in NodeDef nodeDef, in CodeGen.State state, in State graphState)
        {
            switch (Kind)
            {
                case NodeKind.Decl:
                    graphState.GetNodeDecl(DefOrDeclId).CodeGenerator(in this, in state);
                    break;
                case NodeKind.Def:
                    throw new System.Exception("Not implemented!");
            }

            //switch (_type)
            //{
            //    case TypeKind.Bool:
            //        state._opWriter.Bool(_defaultValue._bool);
            //        break;

            //    case TypeKind.F32:
            //        state._opWriter.F32(_defaultValue._float);
            //        break;

            //    case TypeKind.I32:
            //        state._opWriter.I32(_defaultValue._int);
            //        break;

            //    case TypeKind.U8:
            //        state._opWriter.U8(_defaultValue._byte);
            //        break;
            //}
        }
    }
}
