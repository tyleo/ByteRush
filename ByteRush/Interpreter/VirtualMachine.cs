using ByteRush.Utilities;
using System;

namespace ByteRush.Interpreter
{
    public sealed class VirtualMachine
    {
        const int STACK_SIZE_VALUES = 10000;

        private int _instructionPointer;
        private int _objectPointer;
        private int _stackPointer;

        private readonly Value[] _stack = new Value[STACK_SIZE_VALUES];
        private readonly byte[][] _objects;

        public VirtualMachine(byte[] @object)
        {
            _instructionPointer = 0;
            _objectPointer = 0;
            _stackPointer = -1;
            _objects = new byte[][] { @object };
        }

        public void Execute()
        {
            while (_instructionPointer < 500)
            {
                Step();
            }
        }

        public void Reset()
        {
            _instructionPointer = 0;
            _objectPointer = 0;
            _stackPointer = -1;
        }

        public void Step()
        {
            var @object = _objects[_objectPointer];
            var instruction = @object[_instructionPointer];
            _instructionPointer++;

            switch ((Op)instruction)
            {
                case Op.AddIntReg:
                    {
                        var lhs = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);
                        var rhs = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);
                        var storeOffset = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);

                        _stack[_stackPointer - storeOffset]._variant._int =
                            _stack[_stackPointer - lhs]._variant._int + _stack[_stackPointer - rhs]._variant._int;
                    }
                    break;

                case Op.AddIntStack:
                    {
                        var lhs = _stack[_stackPointer];
                        _stackPointer--;
                        var rhs = _stack[_stackPointer];
                        _stack[_stackPointer]._variant._int = lhs._variant._int + rhs._variant._int;
                    }
                    break;

                case Op.Copy:
                    {
                        var fromOffset = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);
                        var toOffset = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);

                        _stack[_stackPointer - toOffset] = _stack[_stackPointer - fromOffset];
                    }
                    break;

                case Op.Get:
                    {
                        var offset = @object[_instructionPointer];

                        var value = _stack[_stackPointer - offset];
                        _stackPointer++;
                        _stack[_stackPointer] = value;

                        _instructionPointer++;
                    }
                    break;

                case Op.Goto:
                    {
                        var address = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer = address;
                    }
                    break;

                case Op.IncIntReg:
                    {
                        var offset = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);
                        _stack[_stackPointer - offset]._variant._int++;
                    }
                    break;

                case Op.IncIntStack:
                    {
                        _stack[_stackPointer]._variant._int++;
                    }
                    break;

                case Op.JumpIfFalse:
                    {
                        var condition = _stack[_stackPointer];
                        _stackPointer--;

                        if (condition._variant._bool)
                        {
                            // Skip the branch
                            _instructionPointer += sizeof(int);
                        }
                        else
                        {
                            var address = ByteUtil.ReadInt(@object, _instructionPointer);
                            _instructionPointer = address;
                        }
                    }
                    break;

                case Op.LessThanRegPush:
                    {
                        var lhsOffset = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);
                        var rhsOffset = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);

                        _stack[_stackPointer + 1]._variant._bool =
                            _stack[_stackPointer - lhsOffset]._variant._int < _stack[_stackPointer - rhsOffset]._variant._int;
                        _stackPointer++;
                    }
                    break;

                case Op.LessThanStack:
                    {
                        var lhs = _stack[_stackPointer];
                        _stackPointer--;
                        var rhs = _stack[_stackPointer];
                        _stack[_stackPointer]._variant._bool = lhs._variant._int < rhs._variant._int;
                    }
                    break;

                case Op.PushBlock:
                    {
                        var size = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer += sizeof(int);
                        for (var i = 0; i < size; ++i)
                        {
                            _stackPointer++;
                            _stack[_stackPointer]._variant._int = ByteUtil.ReadInt(@object, _instructionPointer);
                            _instructionPointer += sizeof(int);
                        }
                    }
                    break;

                case Op.PushInt:
                    {
                        var value = ByteUtil.ReadInt(@object, _instructionPointer);
                        _stackPointer++;
                        _stack[_stackPointer]._variant._int = value;
                        _instructionPointer += sizeof(int);
                    }
                    break;

                case Op.Set:
                    {

                        var offset = @object[_instructionPointer];

                        _stack[_stackPointer - offset] = _stack[_stackPointer];
                        _stackPointer--;

                        _instructionPointer++;
                    }
                    break;

                default:
                    throw new Exception("Unknown instruction.");
            }
        }
    }
}
