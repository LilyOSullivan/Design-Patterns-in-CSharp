using System;
using System.Dynamic;
using ImpromptuInterface;

namespace DynamicNullObject
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

    public class Null<TInterface> : DynamicObject
        where TInterface : class
    {
        public static TInterface Instance => new Null<TInterface>().ActLike<TInterface>();

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true;
        }
    }

    class Program
    {
        static void Main()
        {
            var log = Null<ILog>.Instance;
            var bankAccount = new BankAccount(log);
            bankAccount.Deposit(100);
        }
    }
}
