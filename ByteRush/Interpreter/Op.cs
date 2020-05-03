namespace ByteRush.Interpreter
{
    public enum Op :
        byte
    {
        AddInt,
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
        AddIntReg,
        AddIntStack,
        IncIntStack,
        LessThanStack,
        PushInt
    }

    public static class OpCodeExtensions
    {
        public static byte U8(this Op self) => (byte)self;
    }
}
