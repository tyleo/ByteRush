using ByteRush.Utilities;
using System;
using System.Runtime.CompilerServices;

namespace ByteRush.Interpreter
{
    public sealed class VirtualMachine
    {
        const int STACK_SIZE_VALUES = 10000;

        private int _instructionPointer;
        private int _stackPointer;
        private byte[] _object;

        private readonly Value[] _stack = new Value[STACK_SIZE_VALUES];
        private readonly byte[][] _objects;

        public VirtualMachine(byte[] @object)
        {
            _instructionPointer = 0;
            _stackPointer = 0;
            _objects = new byte[][] { @object };
            _object = _objects[0];
        }

        public void Execute()
        {
            while (_instructionPointer < _object.Length)
            {
                Step();
            }
        }

        public void Reset()
        {
            _instructionPointer = 0;
            _stackPointer = 0;
            _object = _objects[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Value StackSlot(int offset) => ref _stack[_stackPointer - offset];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushValue(Value value)
        {
            _stack[_stackPointer] = value;
            _stackPointer++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Op ReadOp() => (Op)ReadU8();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadOpCodeAddress() => ReadI32();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadStackAddress() => ReadI32();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadI32()
        {
            var result = ByteUtil.ReadI32(_object, _instructionPointer);
            _instructionPointer += sizeof(int);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte ReadU8()
        {
            var result = ByteUtil.ReadU8(_object, _instructionPointer);
            _instructionPointer += sizeof(byte);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort ReadU16()
        {
            var result = ByteUtil.ReadU16(_object, _instructionPointer);
            _instructionPointer += sizeof(ushort);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Value ReadValue() => new Value(ReadI32());

        private void Step()
        {
            var instruction = ReadOp();
            switch (instruction)
            {
                case Op.AddI32:
                    {
                        var lhsOffset = ReadStackAddress();
                        var rhsOffset = ReadStackAddress();
                        var returnOffset = ReadStackAddress();
                        StackSlot(returnOffset)._i32 = StackSlot(lhsOffset)._i32 + StackSlot(rhsOffset)._i32;
                    }
                    break;

                case Op.Copy:
                    {
                        var fromOffset = ReadStackAddress();
                        var toOffset = ReadStackAddress();
                        StackSlot(toOffset) = StackSlot(fromOffset);
                    }
                    break;

                case Op.EnterFunction:
                    {
                        var stackSize = ReadU16();

                        var newStackTop = _stackPointer + stackSize;
                        while (_stackPointer < newStackTop) PushValue(ReadValue());
                    }
                    break;

                case Op.Goto:
                    {
                        var address = ReadOpCodeAddress();
                        _instructionPointer = address;
                    }
                    break;

                case Op.IncI32:
                    {
                        var intOffset = ReadI32();
                        StackSlot(intOffset)._i32++;
                    }
                    break;

                case Op.JumpIfFalse:
                    {
                        var conditionOffset = ReadStackAddress();
                        var jumpAddress = ReadOpCodeAddress();

                        if (!StackSlot(conditionOffset)._bool) _instructionPointer = jumpAddress;
                    }
                    break;

                case Op.LessThanI32:
                    {
                        var lhsOffset = ReadStackAddress();
                        var rhsOffset = ReadStackAddress();
                        var returnOffset = ReadStackAddress();

                        StackSlot(returnOffset)._bool = StackSlot(lhsOffset)._i32 < StackSlot(rhsOffset)._i32;
                    }
                    break;

                default:
                    throw new Exception("Unknown instruction.");
            }
        }
    }
}
