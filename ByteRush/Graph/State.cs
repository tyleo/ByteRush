using ByteRush.Action;
using ByteRush.Graph.Definitions;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;

namespace ByteRush.Graph
{
    public sealed class State
    {
        private readonly CompactedList<INodeDecl> _nodeDecls = CompactedList<INodeDecl>.New();

        public INodeDecl GetNodeDecl(NodeDeclId i) => _nodeDecls[i.Int];
        public NodeDef GetNodeDef(NodeDeclId i) => (NodeDef)GetNodeDecl(i);
        public ref readonly Node GetNode(NodeDeclId nodeDef, NodeId node) =>
            ref GetNodeDef(nodeDef).GetNode(node);
        public ref readonly Node GetNode(in NodeKey i) => ref GetNode(i.NodeDef, i.Node);

        private State() { }

        public static State New() => new State();

        public void Reduce(IAction action)
        {
            switch (action.Kind)
            {
                case ActionKind.AddEdge:
                    {
                        var addConnection = (AddEdge)action;
                        var nodeDef = GetNodeDef(addConnection.NodeDef);
                        nodeDef.AddEdge(in addConnection.From, in addConnection.To);
                    }
                    break;

                case ActionKind.AddNode:
                    {
                        var addNode = (AddNode)action;
                        var nodeDef = GetNodeDef(addNode.NodeDefId);
                        nodeDef.AddNode(Node.New(this, nodeDef, addNode.NodeDeclId));
                    }
                    break;

                case ActionKind.AddNodeDecls:
                    {
                        var addNodeDecls = (AddNodeDecls)action;
                        addNodeDecls.NodeDecls.ForEach(i => _nodeDecls.Add(i));
                    }
                    break;

                case ActionKind.SetDefaultValue:
                    {
                        var setDefaultValue = (SetDefaultValue)action;
                        ref readonly var node = ref GetNode(setDefaultValue.Port.NodeDef, setDefaultValue.Port.Node);
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
