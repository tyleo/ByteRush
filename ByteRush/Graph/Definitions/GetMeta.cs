namespace ByteRush.Graph.Definitions
{
    public sealed class GetMeta
    {
        public string VariableName { get; set; }

        private GetMeta() { }

        public static GetMeta New() => new GetMeta();
    }
}
