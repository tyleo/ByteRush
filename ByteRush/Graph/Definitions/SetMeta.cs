namespace ByteRush.Graph.Definitions
{
    public sealed class SetMeta
    {
        public string VariableName { get; set; }

        private SetMeta() { }

        public static SetMeta New() => new SetMeta();
    }
}
