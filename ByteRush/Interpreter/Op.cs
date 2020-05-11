namespace ByteRush.Interpreter
{
    public enum Op :
        byte
    {
        AddI32,
        CallIntrinsic,
        Copy,
        EnterFunction,
        Goto,
        IncI32,
        JumpIfFalse,
        LessThanI32,
    }

    public static class OpCodeExtensions
    {
        public static byte U8(this Op self) => (byte)self;
    }
}
