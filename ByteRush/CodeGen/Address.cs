using ByteRush.Graph;
using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static ByteRush.Utilities.DebugUtil;

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
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MBool>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MF32>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MI32>> self) => self.Mark<MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MU8>> self) => self.Mark<MStackAddress<MValue>>();
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

    public enum SymbolKind
    {
        Return, // Not implemented
        Parameter,
        Variable,
        Anonymous,
        Constant
    }

    public interface ISymbol<T>
    {
        SymbolKind Kind { get; }
        ISymbol<U> Mark<U>();
        void Release();
    }

    public sealed class ParameterSymbol<T> : ISymbol<T>
    {
        public int Index { get; }

        public SymbolKind Kind => SymbolKind.Parameter;

        private ParameterSymbol(int index) => Index = index;

        public static ParameterSymbol<T> New(int index) => new ParameterSymbol<T>(index);

        public ISymbol<U> Mark<U>() => ParameterSymbol<U>.New(Index);

        public void Release() { }
    }

    public sealed class ConstantSymbol<T> : ISymbol<T>
    {
        public (TypeKind, Value) TypedValue { get; }

        public SymbolKind Kind => SymbolKind.Constant;

        private ConstantSymbol((TypeKind, Value) typedValue) => TypedValue = typedValue;

        public static ConstantSymbol<T> New((TypeKind, Value) typedValue) => new ConstantSymbol<T>(typedValue);

        public ISymbol<U> Mark<U>() => ConstantSymbol<U>.New(TypedValue);

        public void Release() { }
    }

    public sealed class VariableSymbol<T> : ISymbol<T>
    {
        public string Name { get; }

        public SymbolKind Kind => SymbolKind.Variable;

        private VariableSymbol(string name) => Name = name;

        public static VariableSymbol<T> New(string name) => new VariableSymbol<T>(name);

        public ISymbol<U> Mark<U>() => VariableSymbol<U>.New(Name);

        public void Release() { }
    }

    public sealed class AnonymousSymbol<T> : ISymbol<T>
    {
        public int Id { get; }
        private readonly Box<int> _uses;
        private System.Action _release;

        public SymbolKind Kind => SymbolKind.Anonymous;

        private AnonymousSymbol(
            int id,
            Box<int> uses,
            System.Action release
        )
        {
            Id = id;
            _uses = uses;
            _release = release;
        }

        public static AnonymousSymbol<T> New(
            int id,
            int uses,
            System.Action release
        ) => new AnonymousSymbol<T>(id, Box.New(uses), release);

        public ISymbol<U> Mark<U>() => new AnonymousSymbol<U>(Id, _uses, _release);

        public void Release()
        {
            _uses._value--;
            if (_uses._value <= 0)
            {
                _release();
                _release = () => Fail("Releasing anonomyous symbol multiple times!");
            }
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

    public struct PendingOpCodeOnlyStackAddressWrite<T, S>
        where S : ISymbol<T>
    {
        public S Symbol { get; }
        public OpCodeOnlyAddress<MStackAddress<T>> WriteLocation { get; }

        private PendingOpCodeOnlyStackAddressWrite(S symbol, OpCodeOnlyAddress<MStackAddress<T>> writeLocation)
        {
            Symbol = symbol;
            WriteLocation = writeLocation;
        }

        public static PendingOpCodeOnlyStackAddressWrite<T, S> New(S symbol, OpCodeOnlyAddress<MStackAddress<T>> writeLocation) =>
            new PendingOpCodeOnlyStackAddressWrite<T, S>(symbol, writeLocation);
    }

    public static class PendingOpCodeOnlyStackAddressWrite
    {
        public static PendingOpCodeOnlyStackAddressWrite<T, S> New<T, S>(S symbol, OpCodeOnlyAddress<MStackAddress<T>> writeLocation)
            where S : ISymbol<T> => PendingOpCodeOnlyStackAddressWrite<T, S>.New(symbol, writeLocation);
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
