using ByteRush.Utilities.Extensions;
using System;

namespace ByteRush.Graph
{
    public struct InputPortId : IEquatable<InputPortId>
    {
        private InputPortId(int value) => Int = value;

        public static InputPortId New(int value) => new InputPortId(value);

        public bool Equals(InputPortId other) => Int == other.Int;

        public override bool Equals(object obj) => this.EquatableEquals(obj);

        public override int GetHashCode() => Int;

        internal int Int { get; }
    }
}
