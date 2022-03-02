using System;
using System.Threading;
using System.Threading.Tasks;

namespace PerThreadSingleton
{
    public sealed class PerThreadSingleton
    {
        private static ThreadLocal<PerThreadSingleton> threadInstance =
            new(() => new PerThreadSingleton());
        public static PerThreadSingleton Instance => threadInstance.Value;

        public int ThreadId;

        private PerThreadSingleton()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }

    class Program
    {
        static void Main()
        {
            var thread1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Thread1: {PerThreadSingleton.Instance.ThreadId}");
            });

            var thread2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Thread2: {PerThreadSingleton.Instance.ThreadId}");
                Console.WriteLine($"Thread2: {PerThreadSingleton.Instance.ThreadId}");
            });
            Task.WaitAll(thread1, thread2);
        }
    }
}
