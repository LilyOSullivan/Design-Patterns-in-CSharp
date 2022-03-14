using System;
using System.Collections.Generic;
using System.Linq;

namespace CompositeCommand
{
    public class BankAccount
    {
        private int _balance;
        private int _overdraftLimit = -500;

        public BankAccount(int balance = 0)
        {
            this._balance = balance;
        }

        public void Deposit(int amount)
        {
            _balance += amount;
            Console.WriteLine($"Deposited ${amount}, balance is now {_balance}");
        }

        public bool Withdraw(int amount)
        {
            if (_balance - amount >= _overdraftLimit)
            {
                _balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {_balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(_balance)}: {_balance}";
        }
    }

    public interface ICommand
    {
        void Call();
        void Undo();
        bool Success { get; set; }
    }

    public class BankAccountCommand : ICommand
    {
        public bool Success
        {
            get { return _succeeded; }
            set { this._succeeded = value; }
        }
        private readonly BankAccount _account;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action _action;
        private int _amount;
        private bool _succeeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this._account = account;
            this._action = action;
            this._amount = amount;
        }

        public void Call()
        {
            switch (_action)
            {
                case Action.Deposit:
                    _account.Deposit(_amount);
                    _succeeded = true;
                    break;
                case Action.Withdraw:
                    _succeeded = _account.Withdraw(_amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Undo()
        {
            if (!_succeeded) return;
            switch (_action)
            {
                case Action.Deposit:
                    _account.Withdraw(_amount);
                    break;
                case Action.Withdraw:
                    _account.Deposit(_amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class CompositeBankAccountCommand
        : List<BankAccountCommand>, ICommand
    {
        public virtual bool Success
        {
            get
            {
                return this.All(cmd => cmd.Success);
            }
            set
            {
                foreach (var cmd in this)
                {
                    cmd.Success = value;
                }
            }
        }

        public CompositeBankAccountCommand()
        {

        }

        public CompositeBankAccountCommand(IEnumerable<BankAccountCommand> collection)
            : base(collection)
        {

        }

        public virtual void Call()
        {
            ForEach(cmd => cmd.Call());
        }

        public virtual void Undo()
        {
            foreach (var cmd in ((IEnumerable<BankAccountCommand>)this).Reverse())
            {
                if (cmd.Success)
                    cmd.Undo();
            }
        }
    }

    public class MoneyTransferCommand : CompositeBankAccountCommand
    {
        public MoneyTransferCommand(BankAccount from, BankAccount to, int amount)
        {
            AddRange(new[] {
                new BankAccountCommand(from,BankAccountCommand.Action.Withdraw,amount),
                new BankAccountCommand(to,BankAccountCommand.Action.Deposit,amount)
            });
        }

        public override void Call()
        {
            BankAccountCommand last = null;
            foreach (var cmd in this)
            {
                if (last == null || last.Success)
                {
                    cmd.Call();
                    last = cmd;
                }
                else
                {
                    cmd.Undo();
                    break;
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var from = new BankAccount();
            from.Deposit(100);
            var to = new BankAccount();

            var moneyTrasferCommand = new MoneyTransferCommand(from, to, 1000);
            moneyTrasferCommand.Call();

            Console.WriteLine(from);
            Console.WriteLine(to);

            moneyTrasferCommand.Undo();

            Console.WriteLine(from);
            Console.WriteLine(to);
        }
    }
}
