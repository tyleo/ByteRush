using ByteRush.Utilities.Extensions;
using ByteRush.Utilities.Interface;
using System;

namespace ByteRush.Graph
{
    public struct Version : IEquatable<Version>, IInEquatable<Version>
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        private Version(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public Version New(int major, int minor, int patch) => new Version(major, minor, patch);

        public static Version First => new Version(0, 1, 0);

        public bool Equals(in Version other) =>
            Major == other.Major &&
            Minor == other.Minor &&
            Patch == other.Patch;

        public bool Equals(Version other) => Equals(in other);

        public override bool Equals(object other) => IInEquatableExt.InEquatableEquals(in this, other);

        public static bool operator ==(Version lhs, Version rhs) => Equals(lhs, rhs);

        public static bool operator !=(Version lhs, Version rhs) => !(lhs == rhs);

        public override int GetHashCode() =>
            Major.GetHashCode() ^
            Minor.GetHashCode() ^
            Patch.GetHashCode();
    }
}
