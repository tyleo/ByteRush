namespace ByteRush.CodeGen
{
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
}
