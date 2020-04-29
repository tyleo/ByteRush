namespace ByteRush.Interpreter
{
    public enum Op :
        byte
    {
        AddIntReg,
        Copy,
        Goto,
        IncIntReg,
        JumpIfFalse,
        LessThanRegPush,

        Get,
        Set,


        // This may not be any faster than individual pushes.
        PushBlock,

        // Retired?
        AddIntStack,
        IncIntStack,
        LessThanStack,
        PushInt
    }

    public static class OpExtensions
    {
        public static byte Byte(this Op self) => (byte)self;
    }
}
