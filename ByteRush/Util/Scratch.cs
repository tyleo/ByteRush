namespace ByteRush.Util
{
    #region Idea for tagged variant

    //public enum ValueKind
    //{
    //    Int,
    //    Float,
    //    Bool
    //}

    //public struct TaggedVariant
    //{
    //    public readonly Variant _variant;
    //    public readonly ValueKind _tag;

    //    private TaggedVariant(Variant variant, ValueKind tag)
    //    {
    //        _variant = variant;
    //        _tag = tag;
    //    }

    //    public static TaggedVariant Int(int value) => new TaggedVariant(Variant.Int(value), ValueKind.Int);
    //    public static TaggedVariant Float(float value) => new TaggedVariant(Variant.Float(value), ValueKind.Float);
    //    public static TaggedVariant Bool(bool value) => new TaggedVariant(Variant.Bool(value), ValueKind.Bool);
    //}

    #endregion Idea for tagged variant

    #region Some extra byte operations

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static unsafe int ReadInt(byte[] bytes, int intIndex) =>
    //    *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex);

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static unsafe void WriteInt(byte[] bytes, int intIndex, int value) =>
    //    *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex) = value;

    //    public static unsafe long Addr(byte[] bytes) => (long)Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);

    //    public static unsafe int NextIntIndex(byte[] bytes, int startIndex)
    //    {
    //        var startAddress = (long)Marshal.UnsafeAddrOfPinnedArrayElement(bytes, startIndex);

    //        // If this is 0 we can place an int in 0 slots
    //        // If this is 1 we can place an int in 3 slots.
    //        // If this is 2 we can place an int in 2 slots.
    //        // If this is 3 we can place an int in 1 slots.
    //        var startAddressModSize = (int)(startAddress % sizeof(int));

    //        return
    //            startAddressModSize == 0 ?
    //            startIndex :
    //            startIndex + sizeof(int) - startAddressModSize;
    //    }

    //    public static unsafe int ExtractInt(byte[] bytes, int intIndex) =>
    //        *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex);

    //    public static unsafe void InjectInt(byte[] bytes, int intIndex, int value) =>
    //        *(int*)Marshal.UnsafeAddrOfPinnedArrayElement(@bytes, intIndex) = value;

    #endregion Some extra byte operations
}
