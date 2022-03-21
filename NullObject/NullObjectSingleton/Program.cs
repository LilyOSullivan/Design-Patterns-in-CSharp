using System;

namespace NullObjectSingleton
{
    public interface ILog
    {
        void Warn();

        public static ILog Null => NullLog.Instance;

        private sealed class NullLog : ILog
        {
            private static Lazy<NullLog> instance = new(() => new NullLog());

            public static ILog Instance => instance.Value;

            private NullLog()
            {

            }

            public void Warn()
            {

            }
        }
    }

    //public class NullLog : ILog
    //{
    //    private NullLog()
    //    {

    //    }

    //    private static Lazy<NullLog> instance;
    //    public static ILog Instance => instance.Value;

    //    void Warn()
    //    {

    //    }
    //}

    class Program
    {
        static void Main()
        {

        }
    }
}
