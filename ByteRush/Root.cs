using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ByteRush
{
    public enum Op :
        byte
    {
        PushInt,
        JumpIfFalse,
        LessThan,
        AddInt,
        IncInt,
        Get,
        Goto,
        Set
    }

    public static class IntExtensions
    {
        public static byte Byte(this int self) => (byte)self;
    }

    public static class OpExtensions
    {
        public static byte Byte(this Op self) => (byte)self;
    }

    public sealed class ArrayList<T>
    {
        private const int GROWTH_FACTOR = 2;

        private T[] _values;
        private int _size;

        public T[] Inner => _values;

        public int Length => _size;

        private ArrayList(T[] values, int size)
        {
            _values = values;
            _size = size;
        }

        public ArrayList<T> Add(T value)
        {
            EnsureOverhead(1);
            _values[_size] = value;
            _size++;
            return this;
        }

        public static ArrayList<T> New() => new ArrayList<T>(new T[0], 0);

        public ArrayList<T> Grow(int growthAmount)
        {
            EnsureOverhead(growthAmount);
            _size += growthAmount;
            return this;
        }

        public ArrayList<T> EnsureOverhead(int overhead)
        {
            if (_size + overhead > _values.Length)
            {
                var newValues = new T[(_size + overhead) * 2];
                Array.Copy(_values, newValues, _size);
                _values = newValues;
            }
            return this;
        }

        public T[] ToArray() => _values.Take(Length).ToArray();
    }

    public struct IntInserter
    {
        private readonly ArrayList<byte> _bytes;
        private readonly int _index;

        public IntInserter(ArrayList<byte> bytes, int index)
        {
            _bytes = bytes;
            _index = index;
        }

        public void Write(int value) => ByteUtil.WriteInt(_bytes.Inner, _index, value);
    }

    public struct OpWriter
    {
        private readonly ArrayList<byte> _bytes;

        public byte[] GetBytes() => _bytes.ToArray();

        public int GetAddress() => _bytes.Length;

        private void Byte(byte value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(byte));
            ByteUtil.WriteByte(_bytes.Inner, startIndex, value);
        }

        private void Int(int value)
        {
            var startIndex = _bytes.Length;
            _bytes.Grow(sizeof(int));
            ByteUtil.WriteInt(_bytes.Inner, startIndex, value);
        }

        private void Null() => _bytes.Add(0xFF);

        private void Nulls(int amount)
        {
            for (var i = 0; i < amount; ++i) Null();
        }

        public void OpAddInt() => _bytes.Add(Op.AddInt.Byte());

        public void OpGet(byte offset)
        {
            _bytes.Add(Op.Get.Byte());
            _bytes.Add(offset);
        }

        public void OpGoto(int address)
        {
            _bytes.Add(Op.Goto.Byte());
            Int(address);
        }

        public void OpIncInt() => _bytes.Add(Op.IncInt.Byte());

        public IntInserter OpJumpIfFalse()
        {
            _bytes.Add(Op.JumpIfFalse.Byte());
            var insertAddress = GetAddress();
            Int(0);
            return new IntInserter(_bytes, insertAddress);
        }

        public void OpLessThan() => _bytes.Add(Op.LessThan.Byte());

        public void OpPushInt(int value)
        {
            _bytes.Add(Op.PushInt.Byte());
            Int(value);
        }

        public void OpSet(byte offset)
        {
            _bytes.Add(Op.Set.Byte());
            _bytes.Add(offset);
        }

        private void Zero() => _bytes.Add(0);

        private void Zeroes(int amount)
        {
            for (var i = 0; i < amount; ++i) Zero();
        }

        private OpWriter(ArrayList<byte> bytes) => _bytes = bytes;

        public static OpWriter New() => new OpWriter(ArrayList<byte>.New());
    }

    public struct Variant
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Value
        {
            [FieldOffset(0)]
            public float _float;
            [FieldOffset(0)]
            public int _int;
            [FieldOffset(0)]
            public bool _bool;
        }

        public Value value;
        public object reference;

        public T CastRef<T>() => (T)reference;
    }

    public static class ByteUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadByte(byte[] bytes, int index) => bytes[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt(byte[] bytes, int index) =>
            bytes[index + 0] << 0 |
            bytes[index + 1] << 8 |
            bytes[index + 2] << 16 |
            bytes[index + 3] << 24;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByte(byte[] bytes, int index, byte value) => bytes[index] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt(byte[] bytes, int index, int value)
        {
            bytes[index + 0] = (value >> 0).Byte();
            bytes[index + 1] = (value >> 8).Byte();
            bytes[index + 2] = (value >> 16).Byte();
            bytes[index + 3] = (value >> 24).Byte();
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static unsafe int ReadInt(byte[] bytes, int intIndex) =>
        //    *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static unsafe void WriteInt(byte[] bytes, int intIndex, int value) =>
        //    *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex) = value;
    }

    //    public static unsafe long Addr(byte[] bytes) => (long)Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);

    //    public static unsafe int NextIntIndex(byte[] bytes, int startIndex)
    //    {
    //        var startAddress = (long)Marshal.UnsafeAddrOfPinnedArrayElement(bytes, startIndex);

    //        // If this is 0 we can place an int in 0 slots
    //        // If this is 1 we can place an int in 3 slots.
    //        // If this is 2 we can place an int in 2 slots.
    //        // If this is 3 we can place an int in 1 slots.
    //        var startAddressModSize = (int)(startAddress % sizeof(int));

    //        return
    //            startAddressModSize == 0 ?
    //            startIndex :
    //            startIndex + sizeof(int) - startAddressModSize;
    //    }

    //    public static unsafe int ExtractInt(byte[] bytes, int intIndex) =>
    //        *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex);

    //    public static unsafe void InjectInt(byte[] bytes, int intIndex, int value) =>
    //        *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex) = value;
    //

    public sealed class VM
    {
        const int STACK_SIZE_VALUES = 10000;

        private int _instructionPointer;
        private int _objectPointer;
        private int _stackPointer;

        private readonly Variant[] _stack = new Variant[STACK_SIZE_VALUES];
        private readonly byte[][] _objects;

        private readonly Stopwatch _sw = new Stopwatch();
        private readonly double[] _stepCount = new double[10000];
        private int _stepCountIndex;

        public VM(byte[] @object)
        {
            _instructionPointer = 0;
            _objectPointer = 0;
            _stackPointer = 0;
            _objects = new byte[][] { @object };
            _stepCountIndex = 0;
        }

        public void Execute()
        {
            while (_instructionPointer < 500)
            {
                //_sw.Restart();
                Step();
                //_stepCount[_stepCountIndex++] = _sw.ElapsedHighResolutionFrame60s() * 100;
            }
        }

        public void Reset()
        {
            _instructionPointer = 0;
            _objectPointer = 0;
            _stackPointer = 0;
            _stepCountIndex = 0;
        }

        public void Step()
        {
            var @object = _objects[_objectPointer];
            var instruction = @object[_instructionPointer];
            _instructionPointer++;

            switch ((Op)instruction)
            {
                case Op.AddInt:
                    {
                        var lhs = _stack[_stackPointer - 1];
                        _stackPointer--;
                        var rhs = _stack[_stackPointer - 1];
                        _stack[_stackPointer - 1].value._int = lhs.value._int + rhs.value._int;
                    }
                    break;

                case Op.Get:
                    {
                        var offset = @object[_instructionPointer];

                        _stack[_stackPointer] = _stack[_stackPointer - offset - 1];
                        _stackPointer++;

                        _instructionPointer++;
                    }
                    break;

                case Op.Goto:
                    {
                        var address = ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer = address;
                    }
                    break;

                case Op.IncInt:
                    {
                        _stack[_stackPointer - 1].value._int++;
                    }
                    break;

                case Op.JumpIfFalse:
                    {
                        var condition = _stack[_stackPointer - 1];
                        _stackPointer--;

                        var newIp = condition.value._bool ?
                            _instructionPointer + sizeof(int) :
                            ByteUtil.ReadInt(@object, _instructionPointer);
                        _instructionPointer = newIp;
                        //if (condition._value._bool)
                        //{
                        //    // Skip the branch
                        //    _instructionPointer += sizeof(int);
                        //}
                        //else
                        //{
                        //    var address = ByteUtil.ReadInt(@object, _instructionPointer);
                        //    _instructionPointer = address;
                        //}
                    }
                    break;

                case Op.LessThan:
                    {
                        var lhs = _stack[_stackPointer - 1];
                        _stackPointer--;
                        var rhs = _stack[_stackPointer - 1];
                        _stack[_stackPointer - 1].value._bool = lhs.value._int < rhs.value._int;
                    }
                    break;

                case Op.PushInt:
                    {
                        var value = ByteUtil.ReadInt(@object, _instructionPointer);
                        _stack[_stackPointer].value._int = value;
                        _stackPointer++;
                        _instructionPointer += sizeof(int);
                    }
                    break;

                case Op.Set:
                    {

                        var offset = @object[_instructionPointer];

                        _stack[_stackPointer - offset - 1] = _stack[_stackPointer - 1];
                        _stackPointer--;

                        _instructionPointer++;
                    }
                    break;

                default:
                    throw new Exception("Unknown instruction.");
            }
        }
    }

    public static class StopwatchExtensions
    {
        private static readonly double ticksPerSecond = Stopwatch.Frequency;
        private static readonly double secondsPerTick = 1 / ticksPerSecond;

        private static readonly double millisecondsPerSecond = 1000;
        private static readonly double millisecondsPerTick = millisecondsPerSecond * secondsPerTick;

        private static readonly double frame60sPerSecond = 60;
        private static readonly double frame60sPerTick = frame60sPerSecond * secondsPerTick;

        public static double ElapsedHighResolutionFrame60s(this Stopwatch self) => self.ElapsedTicks * frame60sPerTick;

        public static double ElapsedHighResolutionMilliseconds(this Stopwatch self) => self.ElapsedTicks * millisecondsPerTick;

        public static double ElapsedHighResolutionSeconds(this Stopwatch self) => self.ElapsedTicks * secondsPerTick;
    }
}
