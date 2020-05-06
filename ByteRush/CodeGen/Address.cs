using ByteRush.Graph;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace ByteRush.CodeGen
{
    // Markers
    public struct MBool { }
    public struct MFinalOpCodeAddress<T> { }
    public struct MF32 { }
    public struct MI32 { }
    public struct MOpCode { }
    public struct MOpCodeOnlyAddress<T> { }
    public struct MStackAddress<T> { }
    public struct MU8 { }
    public struct MU16 { }
    public struct MUnknown { }
    public struct MValue { }

    public struct StackAddress<T> : IEquatable<StackAddress<T>>
    {
        private StackAddress(int value) => Int = value;

        public static StackAddress<T> New(int value) => new StackAddress<T>(value);

        public StackAddress<U> Mark<U>() => StackAddress<U>.New(Int);

        public bool Equals(StackAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class StackAddress
    {
        public static StackAddress<T> New<T>(int value) => StackAddress<T>.New(value);
    }

    public struct OpCodeOnlyAddress<T> : IEquatable<OpCodeOnlyAddress<T>>
    {
        private OpCodeOnlyAddress(int value) => Int = value;

        public static OpCodeOnlyAddress<T> New(int value) => new OpCodeOnlyAddress<T>(value);

        public OpCodeOnlyAddress<U> Mark<U>() => OpCodeOnlyAddress<U>.New(Int);

        public bool Equals(OpCodeOnlyAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class OpCodeOnlyAddress
    {
        public static OpCodeOnlyAddress<T> New<T>(int value) => OpCodeOnlyAddress<T>.New(value);
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MBool>> self) => self.Mark< MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MF32>> self) => self.Mark< MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MI32>> self) => self.Mark< MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MU8>> self) => self.Mark< MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MBool>> ToBool(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MBool>>();
        public static OpCodeOnlyAddress<MStackAddress<MF32>> ToF32(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MF32>>();
        public static OpCodeOnlyAddress<MStackAddress<MI32>> ToI32(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MI32>>();
        public static OpCodeOnlyAddress<MStackAddress<MU8>> ToU8(this OpCodeOnlyAddress<MStackAddress<MValue>> self) => self.Mark<MStackAddress<MU8>>();
    }

    public struct PreambleAddress<T> : IEquatable<PreambleAddress<T>>
    {
        private PreambleAddress(int value) => Int = value;

        public static PreambleAddress<T> New(int value) => new PreambleAddress<T>(value);

        public bool Equals(PreambleAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class PreambleAddress
    {
        public static PreambleAddress<T> New<T>(int value) => PreambleAddress<T>.New(value);
    }

    public struct FinalOpCodeAddress<T> : IEquatable<FinalOpCodeAddress<T>>
    {
        private FinalOpCodeAddress(int value) => Int = value;

        public static FinalOpCodeAddress<T> New(int value) => new FinalOpCodeAddress<T>(value);

        public FinalOpCodeAddress<U> Mark<U>() => FinalOpCodeAddress<U>.New(Int);

        public static FinalOpCodeAddress<T> From(
            PreambleAddress<MOpCode> preambleEndAddress,
            OpCodeOnlyAddress<T> opCodeOnlyAddress
        ) => New(preambleEndAddress.Int + opCodeOnlyAddress.Int);

        public bool Equals(FinalOpCodeAddress<T> other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        public int Int { get; }
    }

    public static class FinalOpCodeAddress
    {
        public static FinalOpCodeAddress<MOpCode> EndOfProgram => New<MOpCode>(int.MaxValue);
        public static FinalOpCodeAddress<T> From<T>(
            PreambleAddress<MOpCode> preambleEndAddress,
            OpCodeOnlyAddress<T> opCodeOnlyAddress
        ) => FinalOpCodeAddress<T>.From(
            preambleEndAddress,
            opCodeOnlyAddress
        );
        public static FinalOpCodeAddress<T> New<T>(int value) => FinalOpCodeAddress<T>.New(value);
    }

    public interface ISymbol<T>
    {
        ISymbol<U> Mark<U>();
        void Release();
    }

    public sealed class ParameterSymbol<T> : ISymbol<T>
    {
        private readonly int _inner;

        private ParameterSymbol(int inner) => _inner = inner;

        public static ParameterSymbol<T> New(int inner) => new ParameterSymbol<T>(inner);

        public ISymbol<U> Mark<U>() => ParameterSymbol<U>.New(_inner);

        public void Release() { }
    }

    public sealed class ConstantSymbol<T> : ISymbol<T>
    {
        private readonly (TypeKind, Value) _inner;

        private ConstantSymbol((TypeKind, Value) inner) => _inner = inner;

        public static ConstantSymbol<T> New((TypeKind, Value) inner) => new ConstantSymbol<T>(inner);

        public ISymbol<U> Mark<U>() => ConstantSymbol<U>.New(_inner);

        public void Release() { }
    }

    public sealed class VariableSymbol<T> : ISymbol<T>
    {
        private readonly string _inner;

        private VariableSymbol(string inner) => _inner = inner;

        public static VariableSymbol<T> New(string inner) => new VariableSymbol<T>(inner);

        public ISymbol<U> Mark<U>() => VariableSymbol<U>.New(_inner);

        public void Release() { }
    }

    public sealed class AnonymousSymbol<T> : ISymbol<T>
    {
        private readonly int _inner;
        private Box<int> _uses;
        private readonly System.Action _release;

        private AnonymousSymbol(
            int inner,
            Box<int> uses,
            System.Action release
        )
        {
            _inner = inner;
            _uses = uses;
            _release = release;
        }

        public static AnonymousSymbol<T> New(
            int inner,
            int uses,
            System.Action release
        ) => new AnonymousSymbol<T>(inner, Box.New(uses), release);

        public ISymbol<U> Mark<U>() => new AnonymousSymbol<U>(_inner, _uses, _release);

        public void Release()
        {
            _uses._value--;
            if (_uses._value == 0) _release();
        }
    }

    public struct PendingOpCodeOnlyStackAddressWrite<T>
    {
        public ISymbol<T> Symbol { get; }
        public OpCodeOnlyAddress<MStackAddress<T>> WriteLocation { get; }

        private PendingOpCodeOnlyStackAddressWrite(ISymbol<T> symbol, OpCodeOnlyAddress<MStackAddress<T>> writeLocation)
        {
            Symbol = symbol;
            WriteLocation = writeLocation;
        }

        public static PendingOpCodeOnlyStackAddressWrite<T> New(ISymbol<T> symbol, OpCodeOnlyAddress<MStackAddress<T>> writeLocation) =>
            new PendingOpCodeOnlyStackAddressWrite<T>(symbol, writeLocation);
    }

    public struct PendingOpCodeOnlyOpCodeAddressWrite
    {
        public OpCodeOnlyAddress<MOpCode> Address { get; }
        public OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> WriteLocation { get; }

        private PendingOpCodeOnlyOpCodeAddressWrite(
            OpCodeOnlyAddress<MOpCode> address,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> writeLocation
        )
        {
            Address = address;
            WriteLocation = writeLocation;
        }

        public static PendingOpCodeOnlyOpCodeAddressWrite New(
            OpCodeOnlyAddress<MOpCode> address,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> writeLocation
        ) => new PendingOpCodeOnlyOpCodeAddressWrite(address, writeLocation);
    }
}
