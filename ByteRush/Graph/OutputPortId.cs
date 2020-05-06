using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct OutputPortId : IEquatable<OutputPortId>
    {
        private OutputPortId(int value) => Int = value;

        public static OutputPortId New(int value) => new OutputPortId(value);

        public bool Equals(OutputPortId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        internal int Int { get; }
    }
}
