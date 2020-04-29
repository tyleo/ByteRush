using System.Diagnostics;

namespace ByteRush.Extensions
{
    public static class StopwatchExtensions
    {
        private static readonly double ticksPerSecond = Stopwatch.Frequency;
        private static readonly double secondsPerTick = 1 / ticksPerSecond;

        private static readonly double millisecondsPerSecond = 1000;
        private static readonly double millisecondsPerTick = millisecondsPerSecond * secondsPerTick;

        private static readonly double frame60sPerSecond = 60;
        private static readonly double frame60sPerTick = frame60sPerSecond * secondsPerTick;

        public static double ElapsedHighResolutionFrame60s(this Stopwatch self) => self.ElapsedTicks * frame60sPerTick;

        public static double ElapsedHighResolutionMilliseconds(this Stopwatch self) => self.ElapsedTicks * millisecondsPerTick;

        public static double ElapsedHighResolutionSeconds(this Stopwatch self) => self.ElapsedTicks * secondsPerTick;
    }
}
