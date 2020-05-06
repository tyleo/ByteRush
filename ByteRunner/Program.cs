using ByteRush.Action;
using ByteRush.CodeGen;
using ByteRush.Graph.Definitions;
using ByteRush.Utilities.Extensions;
using ByteRush.Interpreter;
using ByteRush.Graph;
using System;
using System.Diagnostics;

using CodeGenState = ByteRush.CodeGen.CodeOnlyState;
using GraphState = ByteRush.Graph.State;
using ByteRush.Utilities;
using System.Reflection.Metadata;

namespace ByteRunner
{
    public static class Program
    {
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
            


            var opWriter = OpCodeWriter.New();
            var bytes = Bytes();
            var vm = new VirtualMachine(bytes);

            var sw = Stopwatch.StartNew();
            vm.Execute();
            var msResult = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult} ms");
            Console.WriteLine($"{frame60Result} % frame");

            sw.Restart();
            var result = Fibonacci(5);
            var msResult2 = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result2 = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult2} ms");
            Console.WriteLine($"{frame60Result2} % frame");
        }

        private static byte[] Bytes()
        {
            var preambleOpWriter = PreambleOpCodeWriter.New();
            var (variables, anonymouses, constants) = preambleOpWriter.EnterFunction(
                4,
                1,
                Util.NewArray(
                    Value.I32(0),
                    Value.I32(1),
                    Value.I32(5)
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

        private static byte[] Compiled()
        {
            var state = GraphState.New();

            state.Reduce(AddNodeDecls.New(
                AddDef.New(),
                ForDef.New(),
                IfDef.New(),
                LessThanDef.New(),
                SetDef.New(),
                NodeDef.New()
            ));

            var addId = NodeDeclId.New(0);
            var forId = NodeDeclId.New(1);
            // var ifId = NodeDeclId.New(2);
            var lessThanId = NodeDeclId.New(3);
            var setId = NodeDeclId.New(4);
            var worldId = NodeDeclId.New(5);

            state.Reduce(AddNode.New(setId, worldId));
            var setLastCurrentNode0 = NodeKey.New(worldId, NodeId.New(0));
            var setLastCurrentPort0 = FullInputPortKey.New(worldId, NodeId.New(0), InputPortId.New(1));
            state.Reduce(SetSetMetaName.New(setLastCurrentNode0, "lastCurrent"));
            state.Reduce(SetDefaultValue.New(setLastCurrentPort0, Value.I32(0)));

            state.Reduce(AddNode.New(setId, worldId));
            var setCurrentNode0 = NodeKey.New(worldId, NodeId.New(1));
            var setCurrentPort0 = FullInputPortKey.New(worldId, NodeId.New(1), InputPortId.New(1));
            state.Reduce(SetSetMetaName.New(setCurrentNode0, "current"));
            state.Reduce(SetDefaultValue.New(setCurrentPort0, Value.I32(0)));

            state.Reduce(AddNode.New(setId, worldId));
            var setNextNode0 = NodeKey.New(worldId, NodeId.New(2));
            var setNextPort0 = FullInputPortKey.New(worldId, NodeId.New(2), InputPortId.New(1));
            state.Reduce(SetSetMetaName.New(setNextNode0, "next"));
            state.Reduce(SetDefaultValue.New(setNextPort0, Value.I32(1)));


            state.Reduce(AddNode.New(forId, worldId));
            var forNode = NodeKey.New(worldId, NodeId.New(3));
            var forFromPort = FullInputPortKey.New(worldId, NodeId.New(3), InputPortId.New(1));
            var forToPort = FullInputPortKey.New(worldId, NodeId.New(3), InputPortId.New(2));
            state.Reduce(SetDefaultValue.New(forFromPort, Value.I32(0)));
            state.Reduce(SetDefaultValue.New(forToPort, Value.I32(50)));


            state.Reduce(AddNode.New(setId, worldId));
            var setLastCurrentNode1 = NodeKey.New(worldId, NodeId.New(4));
            state.Reduce(SetSetMetaName.New(setLastCurrentNode1, "lastCurrent"));

            state.Reduce(AddNode.New(setId, worldId));
            var setCurrentNode1 = NodeKey.New(worldId, NodeId.New(5));
            state.Reduce(SetSetMetaName.New(setCurrentNode1, "current"));

            state.Reduce(AddNode.New(addId, worldId));
            var addNode = NodeKey.New(worldId, NodeId.New(6));

            state.Reduce(AddNode.New(setId, worldId));
            var setNextNode1 = NodeKey.New(worldId, NodeId.New(7));
            state.Reduce(SetSetMetaName.New(setNextNode1, "next"));

            return Array.Empty<byte>();
        }
    }
}
