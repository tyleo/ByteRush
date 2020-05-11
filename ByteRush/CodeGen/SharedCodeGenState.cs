using ByteRush.Graph;
using ByteRush.Interpreter;
using ByteRush.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.CodeGen
{
    public sealed class SharedCodeGenState
    {
        private readonly CompactedList<Intrinsic> _intrinsics = CompactedList<Intrinsic>.New();
        private readonly IDictionary<NodeDeclId, IntrinsicId> _intrinsicIds = new Dictionary<NodeDeclId, IntrinsicId>();

        private readonly Indexer _functions = Indexer.New();
        private readonly IDictionary<NodeDeclId, FunctionId> _functionIds = new Dictionary<NodeDeclId, FunctionId>();

        public IntrinsicId GetIntrinsicId(NodeDeclId longId, Intrinsic value)
        {
            if (_intrinsicIds.TryGetValue(longId, out var intrinsicId)) return intrinsicId;
            var result = IntrinsicId.New((ushort)_intrinsics.Add(value));
            _intrinsicIds.Add(longId, result);
            return result;
        }

        public FunctionId GetFunctionId(NodeDeclId longId)
        {
            if (_functionIds.TryGetValue(longId, out var functionId)) return functionId;
            var result = FunctionId.New((ushort)_functions.GetIndex());
            _functionIds.Add(longId, result);
            return result;
        }

        public Intrinsic[] GetIntrinsics() => _intrinsics.Items().ToArray();

        private SharedCodeGenState() { }

        public static SharedCodeGenState New() => new SharedCodeGenState();
    }
}
