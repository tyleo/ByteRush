using ByteRush.Action;
using ByteRush.CodeGen;
using ByteRush.Graph.Definitions;
using ByteRush.Utilities.Extensions;
using ByteRush.Interpreter;
using ByteRush.Graph;
using System;
using System.Diagnostics;
using ByteRush.Utilities;

using GraphState = ByteRush.Graph.State;

namespace ByteRunner
{
    public static class Program
    {
        private const int COUNT = 50;

        private static int Fibonacci(int iterations)
        {
            var current = 0;
            var next = 1;
            for (var i = 0; i < iterations; ++i)
            {
                var lastCurrent = current;
                current = next;
                next = lastCurrent + next;
            }
            return current;
        }

        private static void Main(string[] args)
        {
            var rawBytes = RawBytes();
            var rawVm = VirtualMachine.New(rawBytes, Util.NewArray<Intrinsic>());
            var rawVm2 = VirtualMachine.New(rawBytes, Util.NewArray<Intrinsic>());
            var rawVm3 = VirtualMachine.New(rawBytes, Util.NewArray<Intrinsic>());

            var compiledBytes = CompiledBytes();
            var compiledVm = VirtualMachine.New(compiledBytes, Util.NewArray<Intrinsic>());
            var compiledVm2 = VirtualMachine.New(compiledBytes, Util.NewArray<Intrinsic>());
            var compiledVm3 = VirtualMachine.New(compiledBytes, Util.NewArray<Intrinsic>());

            var intrinsics = Intrinsic();
            intrinsics.Execute();

            var sw = Stopwatch.StartNew();

            sw.Restart();
            var result = Fibonacci(COUNT);
            var msResult2 = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result2 = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult2} ms");
            Console.WriteLine($"{frame60Result2} % frame");

            compiledVm2.Execute();

            sw.Restart();
            rawVm.Execute();
            var msResult1 = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result1 = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult1} ms");
            Console.WriteLine($"{frame60Result1} % frame");

            rawVm3.Execute();
            compiledVm3.Execute();

            rawVm2.Execute();
            sw.Restart();
            compiledVm.Execute();
            var msResult0 = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result0 = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult0} ms");
            Console.WriteLine($"{frame60Result0} % frame");

            sw.Restart();
            result = Fibonacci(COUNT);
            msResult2 = sw.ElapsedHighResolutionMilliseconds();
            frame60Result2 = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult2} ms");
            Console.WriteLine($"{frame60Result2} % frame");
        }

        private static byte[] RawBytes()
        {
            var preambleOpWriter = PreambleOpCodeWriter.New();
            var (variables, anonymouses, constants) = preambleOpWriter.EnterFunction(
                4,
                1,
                Util.NewArray(
                    Value.I32(0),
                    Value.I32(1),
                    Value.I32(COUNT)
                )
            );

            var varLastCurrent = variables[0];
            var varCurrent = variables[1];
            var varNext = variables[2];
            var varI = variables[3];

            var anon0 = anonymouses[0];

            var const0 = constants[0];
            var const1 = constants[1];
            var const50 = constants[2];

            var opWriter = CodeOnlyOpCodeWriter.New();

            var (_, copy0LastCurrent0Addr, copy0LastCurrentLastCurrentAddr) = opWriter.Copy();
            // int lastCurrent = 0;

            var (_, copy0Current0Addr, copy0CurrentCurrentAddr) = opWriter.Copy();
            // int current = 0;

            var (_, copy1Next1Addr, copy1NextNextAddr) = opWriter.Copy();
            // int next = 1;

            var (_, copy0I0Addr, copy0IIAddr) = opWriter.Copy();
            // int i = 0;


            var (topOfLoop, lessThanLhs, lessThanRhs, lessThanReturn) = opWriter.LessThanI32();
            // _0 = i < 50


            var (_, conditional, endAddress) = opWriter.JumpIfFalse();
            // if (!_0) Goto End

            var (_, copyCurrentLastCurrentCurrentAddr, copyCurrentLastCurrentLastCurrentAddr) = opWriter.Copy();
            // lastCurrent = current;

            var (_, copyNextCurrentNextAddr, copyNextCurrentCurrentAddr) = opWriter.Copy();
            // current = next;

            var (_, addLastCurrentNextNextLhs, addLastCurrentNextNextRhs, addLastCurrentNextNextReturn) = opWriter.AddI32();
            // next = lastCurrent + next;

            var (_, incI) = opWriter.IncI32();
            // i++


            var (_, bottomOfLoop) = opWriter.Goto();
            // goto _0 = i < iterations;


            var finalOpWriter = FinalOpCodeWriter.From(preambleOpWriter, opWriter);

            finalOpWriter.WriteAddress(const0, copy0LastCurrent0Addr);
            finalOpWriter.WriteAddress(varLastCurrent, copy0LastCurrentLastCurrentAddr);

            finalOpWriter.WriteAddress(const0, copy0Current0Addr);
            finalOpWriter.WriteAddress(varCurrent, copy0CurrentCurrentAddr);

            finalOpWriter.WriteAddress(const1, copy1Next1Addr);
            finalOpWriter.WriteAddress(varNext, copy1NextNextAddr);

            finalOpWriter.WriteAddress(const0, copy0I0Addr);
            finalOpWriter.WriteAddress(varI, copy0IIAddr);

            finalOpWriter.WriteAddress(varI, lessThanLhs.ToValue());
            finalOpWriter.WriteAddress(const50, lessThanRhs.ToValue());
            finalOpWriter.WriteAddress(anon0, lessThanReturn.ToValue());

            finalOpWriter.WriteAddress(anon0, conditional.ToValue());
            finalOpWriter.WriteAddress(FinalOpCodeAddress.EndOfProgram, endAddress);

            finalOpWriter.WriteAddress(varCurrent, copyCurrentLastCurrentCurrentAddr);
            finalOpWriter.WriteAddress(varLastCurrent, copyCurrentLastCurrentLastCurrentAddr);

            finalOpWriter.WriteAddress(varNext, copyNextCurrentNextAddr);
            finalOpWriter.WriteAddress(varCurrent, copyNextCurrentCurrentAddr);

            finalOpWriter.WriteAddress(varLastCurrent, addLastCurrentNextNextLhs.ToValue());
            finalOpWriter.WriteAddress(varNext, addLastCurrentNextNextRhs.ToValue());
            finalOpWriter.WriteAddress(varNext, addLastCurrentNextNextReturn.ToValue());

            finalOpWriter.WriteAddress(varI, incI.ToValue());

            finalOpWriter.WriteAddress(topOfLoop, bottomOfLoop);

            return finalOpWriter.GetOpCode();
        }

        private static byte[] CompiledBytes()
        {
            var state = GraphState.New();

            var worldId = Guid.NewGuid().NodeDeclId();
            var world = FunctionDef.New(FullName.FromLibEnd("Test", "World"));
            state.Reduce(AddNodeDecls.New(Def.ALL));
            state.Reduce(AddNodeDecls.New((worldId, world)));

            var nodeId = 0;
            state.Reduce(AddNode.New(Def.SET_ID, worldId));
            var setLastCurrentNode0 = NodeKey.New(worldId, NodeId.New(nodeId++));
            var setLastCurrentPort0 = setLastCurrentNode0.FullInputPortKey(InputPortId.New(1));
            state.Reduce(SetSetMetaName.New(setLastCurrentNode0, "lastCurrent"));
            state.Reduce(SetDefaultValue.New(setLastCurrentPort0, Value.I32(0)));

            state.Reduce(AddNode.New(Def.SET_ID, worldId));
            var setCurrentNode0 = NodeKey.New(worldId, NodeId.New(nodeId++));
            var setCurrentPort0 = setCurrentNode0.FullInputPortKey(InputPortId.New(1));
            state.Reduce(SetSetMetaName.New(setCurrentNode0, "current"));
            state.Reduce(SetDefaultValue.New(setCurrentPort0, Value.I32(0)));


            state.Reduce(AddEdge.New(
                worldId,
                setLastCurrentNode0.OutputPortKey(OutputPortId.New(0)),
                setCurrentNode0.InputPortKey(InputPortId.New(0))
            ));

            state.Reduce(AddNode.New(Def.SET_ID, worldId));
            var setNextNode0 = NodeKey.New(worldId, NodeId.New(nodeId++));
            var setNextPort0 = setNextNode0.FullInputPortKey(InputPortId.New(1));
            state.Reduce(SetSetMetaName.New(setNextNode0, "next"));
            state.Reduce(SetDefaultValue.New(setNextPort0, Value.I32(1)));

            state.Reduce(AddEdge.New(
                worldId,
                setCurrentNode0.OutputPortKey(OutputPortId.New(0)),
                setNextNode0.InputPortKey(InputPortId.New(0))
            ));


            state.Reduce(AddNode.New(Def.FOR_ID, worldId));
            var forNode = NodeKey.New(worldId, NodeId.New(nodeId++));
            var forFromPort = forNode.FullInputPortKey(InputPortId.New(1));
            var forToPort = forNode.FullInputPortKey(InputPortId.New(2));
            state.Reduce(SetDefaultValue.New(forFromPort, Value.I32(0)));
            state.Reduce(SetDefaultValue.New(forToPort, Value.I32(COUNT)));


            state.Reduce(AddEdge.New(
                worldId,
                setNextNode0.OutputPortKey(OutputPortId.New(0)),
                forNode.InputPortKey(InputPortId.New(0))
            ));

            state.Reduce(AddNode.New(Def.GET_ID, worldId));
            var getCurrent = NodeKey.New(worldId, NodeId.New(nodeId++));
            state.Reduce(SetGetMetaName.New(getCurrent, "current"));

            state.Reduce(AddNode.New(Def.GET_ID, worldId));
            var getLastCurrent = NodeKey.New(worldId, NodeId.New(nodeId++));
            state.Reduce(SetGetMetaName.New(getLastCurrent, "lastCurrent"));

            state.Reduce(AddNode.New(Def.GET_ID, worldId));
            var getNext = NodeKey.New(worldId, NodeId.New(nodeId++));
            state.Reduce(SetGetMetaName.New(getNext, "next"));

            state.Reduce(AddNode.New(Def.SET_ID, worldId));
            var setLastCurrentNode1 = NodeKey.New(worldId, NodeId.New(nodeId++));
            state.Reduce(SetSetMetaName.New(setLastCurrentNode1, "lastCurrent"));

            state.Reduce(AddEdge.New(
                worldId,
                forNode.OutputPortKey(OutputPortId.New(0)),
                setLastCurrentNode1.InputPortKey(InputPortId.New(0))
            ));

            state.Reduce(AddEdge.New(
                worldId,
                getCurrent.OutputPortKey(OutputPortId.New(0)),
                setLastCurrentNode1.InputPortKey(InputPortId.New(1))
            ));

            state.Reduce(AddNode.New(Def.SET_ID, worldId));
            var setCurrentNode1 = NodeKey.New(worldId, NodeId.New(nodeId++));
            state.Reduce(SetSetMetaName.New(setCurrentNode1, "current"));

            state.Reduce(AddEdge.New(
                worldId,
                setLastCurrentNode1.OutputPortKey(OutputPortId.New(0)),
                setCurrentNode1.InputPortKey(InputPortId.New(0))
            ));

            state.Reduce(AddEdge.New(
                worldId,
                getNext.OutputPortKey(OutputPortId.New(0)),
                setCurrentNode1.InputPortKey(InputPortId.New(1))
            ));

            state.Reduce(AddNode.New(Def.ADD_ID, worldId));
            var addNode = NodeKey.New(worldId, NodeId.New(nodeId++));

            state.Reduce(AddEdge.New(
                worldId,
                getLastCurrent.OutputPortKey(OutputPortId.New(0)),
                addNode.InputPortKey(InputPortId.New(0))
            ));

            state.Reduce(AddEdge.New(
                worldId,
                getNext.OutputPortKey(OutputPortId.New(0)),
                addNode.InputPortKey(InputPortId.New(1))
            ));

            state.Reduce(AddNode.New(Def.SET_ID, worldId));
            var setNextNode1 = NodeKey.New(worldId, NodeId.New(nodeId++));
            state.Reduce(SetSetMetaName.New(setNextNode1, "next"));

            state.Reduce(AddEdge.New(
                worldId,
                setCurrentNode1.OutputPortKey(OutputPortId.New(0)),
                setNextNode1.InputPortKey(InputPortId.New(0))
            ));

            state.Reduce(AddEdge.New(
                worldId,
                addNode.OutputPortKey(OutputPortId.New(0)),
                setNextNode1.InputPortKey(InputPortId.New(1))
            ));

            return CodeGenState.GenerateCodeForNode(world, state, setLastCurrentNode0.Node);
        }

        private static VirtualMachine Intrinsic()
        {
            var state = GraphState.New();

            var worldId = Guid.NewGuid().NodeDeclId();
            var world = FunctionDef.New(FullName.FromLibEnd("Test", "World"));

            var helloId = Guid.NewGuid().NodeDeclId();
            var hello = IntrinsicDef.New(
                FullName.FromLibEnd("Test", "Hello"),
                (in RetParams inOut) =>
                {
                    var value = inOut.I32Param(0);
                    Console.Write(value);
                    inOut.I32Return(0, value);
                },
                Util.NewArray(("In", TypeKind.I32)),
                Util.NewArray(("Out", TypeKind.I32))
            );

            state.Reduce(AddNodeDecls.New(
                (worldId, world),
                (helloId, hello)
            ));

            state.Reduce(AddNode.New(helloId, worldId));
            var helloNodeId = NodeId.New(0);
            state.Reduce(SetDefaultValue.New(worldId.FullInputPortKey(helloNodeId, InputPortId.New(0)), new Value(5)));

            var sharedState = SharedCodeGenState.New();
            var bytes = CodeGenState.GenerateCodeForNode(world, state, helloNodeId, sharedState);
            var intrinsics = sharedState.GetIntrinsics();

            return VirtualMachine.New(bytes, intrinsics);
        }
    }
}
