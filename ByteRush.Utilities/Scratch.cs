namespace ByteRush.Utilities
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

    // Memory Types
    // stack
    // instruction stream (constants)

    // Variables:
    // * Per stack-frame allocated
    // * No such thing as a register, just fixed stack locations
    // * No instructions should push or pop from the stack frame accept to create the stack frame
    // * Constants are pushed onto the stack rather than read from the instruction stream

    // First pass (finding all of the write locations in instructions):
    // * Write ops to a stream with memTypes. This will not be the final stream.
    // * Write 0xFF for the address of each symbol (constant, variable or anonymous).
    // * Write 0xFF for the result of each jump but:
    //   * We may need to create an InstructionStreamInserter which uses an instruction_id to create an InstructionStreamInserter to actully write values.
    //   * InstructionStreamInserter { goto_address_location } -> InstructionStreamInserter2 { goto_address_location, goto_instruction_id }
    // * There shouldn't be any addresses in the instruction stream at the end of the first pass.

    // Second pass (calculate stack addresses):
    // * Bucket ConstantSymbols, VariableSymbols and AnonymousSymbols
    // * Count the amount of symbols we have, symbol_base. Stack addresses written to the instruction stream will be top_of_stack - symbol_base + symbol_id
    // * We will primairly write symbol_base + symbol_id and subtract from top_of_stack in the VM
    // * Write the (single) push instruction which allocates the stack. This instruction will be like:
    // [op][size][constants][size][variables]


    // Third pass (calculate jump addresses):
    // * Save the current address, this will be the instruction_base
    // * Copy the rest of the instruction stream into the final stream
    // * Instruction addresses are written to the instruction stream as instruction_base + goto_instruction_id

    // * Write all ConstantSymbols into the final instruction stream. Update the addresses in the old instruction stream.
    // * Write all VariableSymbols into the final instruction stream. Update the addresses in the old instruction stream.
    // * Write all AnonymousSymbols into the final instruction stream. Update the addresses in the old instruction stream.
    // * Write all of InstructionStreamInserter2s into their goto_address_locations

    // * Graph compilation of Function (function)
    // * If pure start at end, if impure start at front
    // * Pure find the end node
    //   * Data back until the first node is found. Generate data back
    //   from each consecutive node. Never clear the _expressionContext.
    // * Impure find the start node.
    //   * Exec forwards until a merge or end node is found. At each node generate data back to get data for
    //   the node.
    //   * Once all the data back have been generated bump the _expressionContext and clear all of the
    //   new nodes out of _generatedNodes.




    // Add instruction:
    // [ 1][       1][4][4][4]
    // [op][memTypes][a][b][c]

    // Calling convention:

    // Caller: Pushes [return address][arguments]
    //         Runs Callee
    //         Pops [return values] into [variables] and [anonymous]

    // Callee: Pushes [variables][anonymous][constants]
    //         Runs
    //         Pops [return address][arguments][variables][anonymous][constants]
    //         Pushes [return values]


    // How do we know which anonymous symbol to use?
    // Need a way to mark "is in use"
    // Add(Add(1, 2), Add(3, 4))
    // [1][2][3][4][][]
    // .0 = Add(1, 2)
    // .1 = Add(3, 4)
    // .0 = Add(1, 2)
    // The current node needs to tell previous nodes the first avaliable anonymous
    // We generate code for expressions back-to-front




    // Should the GenerateCode signature on a node pass the node its relevant input/output addresess?
    // In other words, should we do all of the graph crawling to give a node its addresses? This would 
    // mean that:
    // 1) All inputs are evaluated top to bottom. For impure nodes, expressions are reset when the bottom input is
    // reached.
    // 2) The node generates its code.
    // 3) All outputs are evaluated and branch results are returned.





    // Likely need to put objects in a separate stack
}
