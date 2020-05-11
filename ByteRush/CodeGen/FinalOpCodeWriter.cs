using System.Linq;

namespace ByteRush.CodeGen
{
    public sealed class FinalOpCodeWriter
    {
        private readonly PreambleAddress<MOpCode> _preambleEnd;
        private readonly OpCodeWriter _opWriter;

        private FinalOpCodeWriter(
            PreambleAddress<MOpCode> preambleEnd,
            OpCodeWriter opWriter
        )
        {
            _preambleEnd = preambleEnd;
            _opWriter = opWriter;
        }

        public static FinalOpCodeWriter From(
            PreambleOpCodeWriter preamble,
            CodeOnlyOpCodeWriter codeOnly
        )
        {
            var preambleEnd = preamble.GetAddress();
            var opWriter = OpCodeWriter.From(
                preamble.GetOpCode().Concat(codeOnly.GetOpCode()).ToArray()
            );
            return New(preambleEnd, opWriter);
        }

        private FinalOpCodeAddress<T> NewFinalOpCodeAddress<T>(OpCodeOnlyAddress<T> address) =>
            FinalOpCodeAddress.From(_preambleEnd, address);

        public static FinalOpCodeWriter New(
            PreambleAddress<MOpCode> preambleEnd,
            OpCodeWriter opWriter
        ) => new FinalOpCodeWriter(preambleEnd, opWriter);

        public byte[] GetOpCode() => _opWriter.GetOpCode();

        public void WriteAddress(
            FinalOpCodeAddress<MOpCode> from,
            FinalOpCodeAddress<MFinalOpCodeAddress<MOpCode>> to
        ) => _opWriter.WriteAddress(from, to);

        public void WriteAddress(
            FinalOpCodeAddress<MOpCode> from,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> to
        ) => WriteAddress(
            from,
            NewFinalOpCodeAddress(to)
        );

        public void WriteAddress(
            OpCodeOnlyAddress<MOpCode> from,
            FinalOpCodeAddress<MFinalOpCodeAddress<MOpCode>> to
        ) => WriteAddress(
            NewFinalOpCodeAddress(from),
            to
        );

        public void WriteAddress(
            OpCodeOnlyAddress<MOpCode> from,
            OpCodeOnlyAddress<MFinalOpCodeAddress<MOpCode>> to
        ) => WriteAddress(
            NewFinalOpCodeAddress(from),
            NewFinalOpCodeAddress(to)
        );

        public void WriteAddress<T>(
            StackAddress<T> from,
            FinalOpCodeAddress<MStackAddress<T>> to
        ) => _opWriter.WriteAddress(from, to);

        public void WriteAddress<T>(
            StackAddress<T> from,
            OpCodeOnlyAddress<MStackAddress<T>> to
        ) => _opWriter.WriteAddress(
            from,
            NewFinalOpCodeAddress(to)
        );
    }
}
