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

        public static FinalOpCodeWriter New(
            PreambleAddress<MOpCode> preambleEnd,
            OpCodeWriter opWriter
        ) => new FinalOpCodeWriter(preambleEnd, opWriter);

        public byte[] GetOpCode() => _opWriter.GetOpCode();

        public void WriteAddress<T>(
            FinalOpCodeAddress<T> from,
            FinalOpCodeAddress<MFinalOpCodeAddress<T>> to
        ) => _opWriter.WriteAddress(from, to);

        public void WriteAddress<T>(
            FinalOpCodeAddress<T> from,
            OpCodeOnlyAddress<MFinalOpCodeAddress<T>> to
        ) => WriteAddress(
            from,
            FinalOpCodeAddress.From(_preambleEnd, to)
        );

        public void WriteAddress<T>(
            OpCodeOnlyAddress<T> from,
            FinalOpCodeAddress<MFinalOpCodeAddress<T>> to
        ) => WriteAddress(
            FinalOpCodeAddress.From(_preambleEnd, from),
            to
        );

        public void WriteAddress<T>(
            OpCodeOnlyAddress<T> from,
            OpCodeOnlyAddress<MFinalOpCodeAddress<T>> to
        ) => WriteAddress(
            FinalOpCodeAddress.From(_preambleEnd, from),
            FinalOpCodeAddress.From(_preambleEnd, to)
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
            FinalOpCodeAddress.From(_preambleEnd, to)
        );
    }
}
