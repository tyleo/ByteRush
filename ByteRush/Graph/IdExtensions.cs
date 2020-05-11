using System.Runtime.CompilerServices;

namespace ByteRush.Graph
{
    public static class IdExtensions
    {
        public static NodeDeclId NodeDeclId(this FullName self) =>
            ByteRush.Graph.NodeDeclId.New(self);

        public static NodeKey NodeKey(this FullInputPortKey self) =>
            Graph.NodeKey.New(self.Function, self.Node);

        public static NodeKey NodeKey(this FullOutputPortKey self) =>
            Graph.NodeKey.New(self.Function, self.Node);


        public static InputPortKey InputPortKey(this InputPortId self, NodeId node) =>
            Graph.InputPortKey.New(node, self);

        public static InputPortKey InputPortKey(this NodeId self, InputPortId port) =>
            Graph.InputPortKey.New(self, port);

        public static InputPortKey InputPortKey(this NodeKey self, InputPortId port) =>
            Graph.InputPortKey.New(self.Node, port);

        public static InputPortKey InputPortKey(this InputPortId self, NodeKey nodeKey) =>
            Graph.InputPortKey.New(nodeKey.Node, self);

        public static InputPortKey InputPortKey(this FullInputPortKey self) =>
            Graph.InputPortKey.New(self.Node, self.Port);


        public static OutputPortKey OutputPortKey(this OutputPortId self, NodeId node) =>
            Graph.OutputPortKey.New(node, self);

        public static OutputPortKey OutputPortKey(this NodeId self, OutputPortId port) =>
            Graph.OutputPortKey.New(self, port);

        public static OutputPortKey OutputPortKey(this NodeKey self, OutputPortId port) =>
            Graph.OutputPortKey.New(self.Node, port);

        public static OutputPortKey OutputPortKey(this OutputPortId self, NodeKey nodeKey) =>
            Graph.OutputPortKey.New(nodeKey.Node, self);

        public static OutputPortKey OutputPortKey(this FullOutputPortKey self) =>
            Graph.OutputPortKey.New(self.Node, self.Port);


        public static FullInputPortKey FullInputPortKey(this InputPortId self, NodeDeclId function, NodeId node) =>
            Graph.FullInputPortKey.New(function, node, self);

        public static FullInputPortKey FullInputPortKey(this NodeId self, NodeDeclId function, InputPortId port) =>
            Graph.FullInputPortKey.New(function, self, port);

        public static FullInputPortKey FullInputPortKey(this NodeDeclId self, NodeId node, InputPortId port) =>
            Graph.FullInputPortKey.New(self, node, port);

        public static FullInputPortKey FullInputPortKey(this InputPortId self, NodeKey nodeKey) =>
            Graph.FullInputPortKey.New(nodeKey.Function, nodeKey.Node, self);

        public static FullInputPortKey FullInputPortKey(this NodeKey self, InputPortId portPort) =>
            Graph.FullInputPortKey.New(self.Function, self.Node, portPort);

        public static FullInputPortKey FullInputPortKey(this InputPortKey self, NodeDeclId function) =>
            Graph.FullInputPortKey.New(function, self.Node, self.Port);

        public static FullInputPortKey FullInputPortKey(this NodeDeclId self, InputPortKey inputPortKey) =>
            Graph.FullInputPortKey.New(self, inputPortKey.Node, inputPortKey.Port);


        public static FullOutputPortKey FullOutputPortKey(this OutputPortId self, NodeDeclId function, NodeId node) =>
            Graph.FullOutputPortKey.New(function, node, self);

        public static FullOutputPortKey FullOutputPortKey(this NodeId self, NodeDeclId function, OutputPortId port) =>
            Graph.FullOutputPortKey.New(function, self, port);

        public static FullOutputPortKey FullOutputPortKey(this NodeDeclId self, NodeId node, OutputPortId port) =>
            Graph.FullOutputPortKey.New(self, node, port);

        public static FullOutputPortKey FullOutputPortKey(this OutputPortId self, NodeKey nodeKey) =>
            Graph.FullOutputPortKey.New(nodeKey.Function, nodeKey.Node, self);

        public static FullOutputPortKey FullOutputPortKey(this NodeKey self, OutputPortId portPort) =>
            Graph.FullOutputPortKey.New(self.Function, self.Node, portPort);

        public static FullOutputPortKey FullOutputPortKey(this OutputPortKey self, NodeDeclId function) =>
            Graph.FullOutputPortKey.New(function, self.Node, self.Port);

        public static FullOutputPortKey FullOutputPortKey(this NodeDeclId self, OutputPortKey outputPortKey) =>
            Graph.FullOutputPortKey.New(self, outputPortKey.Node, outputPortKey.Port);
    }
}
