using ByteRush.Action;
using ByteRush.Graph.Definitions;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System.Collections.Generic;

namespace ByteRush.Graph
{
    public sealed class State
    {
        private readonly Dictionary<NodeDeclId, INodeDecl> _nodeDecls = new Dictionary<NodeDeclId, INodeDecl>();

        public INodeDecl GetNodeDecl(NodeDeclId i) => _nodeDecls[i];
        public FunctionDef GetFunction(NodeDeclId i) => (FunctionDef)GetNodeDecl(i);
        public ref readonly Node GetNode(NodeDeclId function, NodeId node) =>
            ref GetFunction(function).GetNode(node);
        public ref readonly Node GetNode(in NodeKey i) => ref GetNode(i.Function, i.Node);

        private State() { }

        public static State New() => new State();

        public void Reduce(IAction action)
        {
            switch (action.Kind)
            {
                case ActionKind.AddEdge:
                    {
                        var addConnection = (AddEdge)action;
                        var function = GetFunction(addConnection.Function);
                        function.AddEdge(in addConnection.From, in addConnection.To);
                    }
                    break;

                case ActionKind.AddNode:
                    {
                        var addNode = (AddNode)action;
                        var function = GetFunction(addNode.FunctionId);
                        function.AddNode(Node.New(this, function, addNode.NodeDeclId));
                    }
                    break;

                case ActionKind.AddNodeDecls:
                    {
                        var addNodeDecls = (AddNodeDecls)action;
                        addNodeDecls.NodeDecls.ForEach(i => _nodeDecls.Add(NodeDeclId.New(i.FullName), i));
                    }
                    break;

                case ActionKind.SetDefaultValue:
                    {
                        var setDefaultValue = (SetDefaultValue)action;
                        ref readonly var node = ref GetNode(setDefaultValue.Port.Function, setDefaultValue.Port.Node);
                        node.SetDefaultValue(setDefaultValue.Port.Port, setDefaultValue.Value);
                    }
                    break;

                case ActionKind.SetGetMetaName:
                    {
                        var setGetMetaName = (SetGetMetaName)action;
                        ref readonly var getNode = ref GetNode(setGetMetaName.Node);
                        var getMeta = getNode.Meta<GetMeta>();
                        getMeta.VariableName = setGetMetaName.Name;
                    }
                    break;

                case ActionKind.SetSetMetaName:
                    {
                        var setSetMetaName = (SetSetMetaName)action;
                        ref readonly var setNode = ref GetNode(setSetMetaName.Node);
                        var setMeta = setNode.Meta<SetMeta>();
                        setMeta.VariableName = setSetMetaName.Name;
                    }
                    break;
            }
        }
    }
}
