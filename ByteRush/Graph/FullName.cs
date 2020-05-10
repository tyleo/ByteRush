using ByteRush.Utilities.Extensions;
using System;
using System.Linq;
using System.Text;

namespace ByteRush.Graph
{
    public sealed class FullName : IEquatable<FullName>
    {
        private readonly Version _version;
        public ref readonly Version Version => ref _version;
        public string Library { get; }
        public string[] Path { get; }
        public string End { get; }

        private FullName(in Version version, string library, string[] path, string end)
        {
            _version = version;
            Library = library;
            Path = path;
            End = end;
        }

        public static FullName New(in Version version, string library, string[] path, string end) =>
            new FullName(in version, library, path, end);

        public static FullName FromLibEnd(string library, string end) =>
            New(Version.First, library, Array.Empty<string>(), end);

        public bool Equals(FullName other) =>
            _version == other._version &&
            Library == other.Library &&
            Path.SequenceEqual(other.Path) &&
            End.SequenceEqual(other.End);

        public override bool Equals(object other) => this.EquatableEquals(other);

        public static bool operator ==(FullName lhs, FullName rhs) => Equals(lhs, rhs);

        public static bool operator !=(FullName lhs, FullName rhs) => !(lhs == rhs);

        public override int GetHashCode() =>
            _version.GetHashCode() ^
            Library.GetHashCode() ^
            Path.SequenceGetHashCode() ^
            End.GetHashCode();

        public override string ToString()
        {
            var sb = new StringBuilder(
                5 +
                sizeof(int) * 2 * 3 +
                Library.Length +
                Path.Sum(i => i.Length + 1) +
                End.Length
            );

            sb.Append(Version.Major);
            sb.Append('.');
            sb.Append(Version.Minor);
            sb.Append('.');
            sb.Append(Version.Patch);
            sb.Append(':');
            sb.Append(Library);
            sb.Append('.');
            Library.ForEach(s => sb.Append(s).Append('.'));
            sb.Append(End);

            return sb.ToString();
        }
    }
}
