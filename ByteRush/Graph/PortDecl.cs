namespace ByteRush.Graph
{
    public sealed class PortDecl
    {
        public string Name { get; }
        public TypeKind Type { get; }

        private PortDecl(string name, TypeKind type)
        {
            Name = name;
            Type = type;
        }

        public static PortDecl New(string name, TypeKind type) =>
            new PortDecl(name, type);
    }
}
