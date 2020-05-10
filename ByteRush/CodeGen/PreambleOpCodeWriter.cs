using ByteRush.Interpreter;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System.Linq;

namespace ByteRush.CodeGen
{
    public sealed class PreambleOpCodeWriter
    {
        private readonly OpCodeWriter _opWriter;

        private PreambleOpCodeWriter() => _opWriter = OpCodeWriter.New();

        public static PreambleOpCodeWriter New() => new PreambleOpCodeWriter();

        public PreambleAddress<MOpCode> GetAddress() => PreambleAddress<MOpCode>.New(_opWriter.GetAddress());

        public byte[] GetOpCode() => _opWriter.GetOpCode();

        public (
            StackAddress<MValue>[] Variables,
            StackAddress<MValue>[] Anonymouses,
            StackAddress<MValue>[] Constants
        ) EnterFunction(int numVariables, int numAnonymouses, Value[] constants)
        {
            var stackSize = (ushort)(numVariables + numAnonymouses + constants.Length);
            _opWriter.Op(Op.EnterFunction);
            _opWriter.U16(stackSize);
            _opWriter.Values(IEnumerableExt.Default<Value?>(numVariables));
            _opWriter.Values(IEnumerableExt.Default<Value?>(numAnonymouses));
            _opWriter.Values(constants.Cast<Value?>());

            return (
                numVariables.Enumerate().Select(i => StackAddress<MValue>.New(numVariables + numAnonymouses + constants.Length - 1 - i)).ToArray(),
                numAnonymouses.Enumerate().Select(i => StackAddress<MValue>.New(numAnonymouses + constants.Length - 1 - i)).ToArray(),
                constants.Length.Enumerate().Select(i => StackAddress<MValue>.New(constants.Length - 1 - i)).ToArray()
            );
        }
    }
}
