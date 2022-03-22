using System;
using Autofac;
using JetBrains.Annotations;

namespace NullObject
{
    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    public class ConsoleLog : ILog
    {
        void ILog.Info(string msg)
        {
            Console.WriteLine(msg);
        }

        void ILog.Warn(string msg)
        {
            Console.WriteLine($"[Warning] {msg}");
        }
    }

    public class BankAccount
    {
        private ILog _log;
        private int _balance;

        public BankAccount(ILog log)
        {
            _log = log;
        }

        public void Deposit(int amount)
        {
            _balance += amount;
            _log?.Info($"Deposited {amount}, balance is {_balance}");
        }
    }

    public class NullLog : ILog
    {
        public void Info(string msg)
        {
            
        }

        public void Warn(string msg)
        {
            
        }
    }

    class Program
    {
        static void Main()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<BankAccount>();
            containerBuilder.RegisterType<NullLog>().As<ILog>();
            using (var container = containerBuilder.Build())
            {
                var bankAccount = container.Resolve<BankAccount>();
                bankAccount.Deposit(100);
            }
        }
    }
}
