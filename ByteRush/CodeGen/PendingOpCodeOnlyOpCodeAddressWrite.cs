namespace ByteRush.CodeGen
{
    public struct PendingOpCodeOnlyOpCodeAddressWrite
    {
        public OpCodeOnlyAddress<MOpCode> Address { get; }
        public OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> WriteLocation { get; }

        private PendingOpCodeOnlyOpCodeAddressWrite(
            OpCodeOnlyAddress<MOpCode> address,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> writeLocation
        )
        {
            Address = address;
            WriteLocation = writeLocation;
        }

        public static PendingOpCodeOnlyOpCodeAddressWrite New(
            OpCodeOnlyAddress<MOpCode> address,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> writeLocation
        ) => new PendingOpCodeOnlyOpCodeAddressWrite(address, writeLocation);
    }
}
