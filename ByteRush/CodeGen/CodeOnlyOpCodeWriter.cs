using ByteRush.Utilities;
using ByteRush.Interpreter;
using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.CodeGen
{
    public sealed class CodeOnlyOpCodeWriter
    {
        private readonly OpCodeWriter _opWriter;

        private CodeOnlyOpCodeWriter() => _opWriter = OpCodeWriter.New();

        public static CodeOnlyOpCodeWriter New() => new CodeOnlyOpCodeWriter();

        public OpCodeOnlyAddress<MOpCode> GetAddress() => OpCodeOnlyAddress<MOpCode>.New(_opWriter.GetAddress());

        public byte[] GetOpCode() => _opWriter.GetOpCode();

        public (
            OpCodeOnlyAddress<MOpCode> Address,
            OpCodeOnlyAddress<MStackAddress<MI32>> Lhs,
            OpCodeOnlyAddress<MStackAddress<MI32>> Rhs,
            OpCodeOnlyAddress<MStackAddress<MI32>> Return
        ) AddI32() => (
            _opWriter.Op(Op.AddI32),
            _opWriter.StackAddress<MI32>(),
            _opWriter.StackAddress<MI32>(),
            _opWriter.StackAddress<MI32>()
        );

        public (
            OpCodeOnlyAddress<MOpCode> Address,
            OpCodeOnlyAddress<MStackAddress<MValue>> From,
            OpCodeOnlyAddress<MStackAddress<MValue>> To
        ) Copy() => (
            _opWriter.Op(Op.Copy),
            _opWriter.StackAddress<MValue>(),
            _opWriter.StackAddress<MValue>()
        );

        public (
            OpCodeOnlyAddress<MOpCode> Address,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> To
        ) Goto() => (
            _opWriter.Op(Op.Goto),
            _opWriter.OpCodeAddress<MOpCode>()
        );

        public (
            OpCodeOnlyAddress<MOpCode> Address,
            OpCodeOnlyAddress<MStackAddress<MBool>> Condition,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> JumpAddress
        ) JumpIfFalse() => (
            _opWriter.Op(Op.JumpIfFalse),
            _opWriter.StackAddress<MBool>(),
            _opWriter.OpCodeAddress<MOpCode>()
        );

        public (
            OpCodeOnlyAddress<MOpCode> Address,
            OpCodeOnlyAddress<MStackAddress<MI32>> Int
        ) IncI32() => (
            _opWriter.Op(Op.IncI32),
            _opWriter.StackAddress<MI32>()
        );

        public (
            OpCodeOnlyAddress<MOpCode> Address,
            OpCodeOnlyAddress<MStackAddress<MI32>> Lhs,
            OpCodeOnlyAddress<MStackAddress<MI32>> Rhs,
            OpCodeOnlyAddress<MStackAddress<MBool>> Return
        ) LessThanI32() => (
            _opWriter.Op(Op.LessThanI32),
            _opWriter.StackAddress<MI32>(),
            _opWriter.StackAddress<MI32>(),
            _opWriter.StackAddress<MBool>()
        );
    }
}
