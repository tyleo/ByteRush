using ByteRush;
using System;
using System.Diagnostics;

namespace ByteRunner
{


    public static class Program
    {
        private static int Fibonacci(int iterations)
        {
            var current = 0;
            var next = 1;
            for (var i = 0; i < iterations; ++i)
            {
                var lastCurrent = current;
                current = next;
                next = lastCurrent + next;
            }
            return current;
        }

        private static void Main(string[] args)
        {
            var opWriter = OpWriter.New();
            //opWriter.OpPushBlock(
            //    0,
            //    0,
            //    1,
            //    0,
            //    5
            //);
            opWriter.OpPushInt(0);        // lastCurrent
            opWriter.OpPushInt(0);        // current
            opWriter.OpPushInt(1);        // next
            opWriter.OpPushInt(0);        // i
            opWriter.OpPushInt(5);        // iterations

            var gotoAddr = opWriter.GetAddress();
            opWriter.OpGet(0);            // Push iterations
            opWriter.OpGet(2);            // Push 0
            opWriter.OpLessThan();        // Push 0 < iterations
            opWriter.OpJumpIfFalse()
                .Write(1000);             // Goto 1000 if false
            opWriter.OpGet(1);            // Get i
            opWriter.OpIncInt();          // Inc i
            opWriter.OpSet(2);            // Set i

            opWriter.OpGet(3);            // Get current;
            opWriter.OpSet(5);            // Set last = current

            opWriter.OpGet(2);            // Get next;
            opWriter.OpSet(4);            // Set current = next

            opWriter.OpGet(4);            // Get lastCurrent
            opWriter.OpGet(4);            // Get next
            opWriter.OpAddInt();          // lastCurrent + next
            opWriter.OpSet(3);            // next = lastCurrent + next

            opWriter.OpGoto(25);         // Goto top of loop

            var bytes = opWriter.GetBytes();
            var vm = new VM(bytes);

            var sw = Stopwatch.StartNew();
            vm.Execute();
            var msResult = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult} ms");
            Console.WriteLine($"{frame60Result} % frame");

            sw.Restart();
            var result = Fibonacci(5);
            var msResult2 = sw.ElapsedHighResolutionMilliseconds();
            var frame60Result2 = sw.ElapsedHighResolutionFrame60s() * 100;
            Console.WriteLine($"{msResult2} ms");
            Console.WriteLine($"{frame60Result2} % frame");
        }
    }
}
