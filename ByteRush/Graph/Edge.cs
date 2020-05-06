namespace ByteRush.Graph
{
    public struct Edge
    {
        public readonly OutputPortKey _from;
        public readonly InputPortKey _to;

        private Edge(in OutputPortKey from, in InputPortKey to)
        {
            _from = from;
            _to = to;
        }

        public static Edge New(in OutputPortKey from, in InputPortKey to) =>
            new Edge(in from, in to);
    }
}
