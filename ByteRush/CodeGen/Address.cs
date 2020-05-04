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
    public struct MI32 { }
    public struct MOpCode { }
    public struct MOpCodeOnlyAddress<T> { }
    public struct MStackAddress<T> { }
    public struct MU8 { }
    public struct MU16 { }
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
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MI32>> self) => self.Mark< MStackAddress<MValue>>();
        public static OpCodeOnlyAddress<MStackAddress<MValue>> ToValue(this OpCodeOnlyAddress<MStackAddress<MU8>> self) => self.Mark< MStackAddress<MValue>>();
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

    public struct InstructionStreamInserter
    {
        private readonly int _address;
    }

    public interface IStackAddressInserter
    {
        
    }

    public sealed class ConstantAddressInserter : IStackAddressInserter
    {
        private readonly InstructionStreamInserter _writeLocation;
    }

    public sealed class VariableAddressInserter : IStackAddressInserter
    {
        private readonly InstructionStreamInserter _writeLocation;
    }

    public interface ISymbol
    {
        void Release();
    }

    public sealed class ConstantSymbol : ISymbol
    {
        private readonly (TypeKind, Value) inner;

        public void Release() { }
    }

    public sealed class VariableSymbol : ISymbol
    {
        private readonly string _inner;

        private VariableSymbol(string inner) => _inner = inner;

        public static VariableSymbol New(string inner) => new VariableSymbol(inner);

        public void Release() { }
    }

    public sealed class AnonymousSymbol : ISymbol
    {
        private readonly int _inner;
        private readonly System.Action _release;

        private AnonymousSymbol(int inner, System.Action release)
        {
            _inner = inner;
            _release = release;
        }

        public static AnonymousSymbol New(int inner, System.Action release) => new AnonymousSymbol(inner, release);

        public void Release() => _release?.Invoke();
    }

    public sealed class AddressRequests
    {
        // Seen constant addresses and the locations they need to be inserted.
        Dictionary<ISymbol, ArrayList<IStackAddressInserter>> stackAddressRequests;
    }
}
