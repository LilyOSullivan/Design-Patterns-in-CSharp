using System;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    public class BankAccount
    {
        private int _balance;
        private int _overdraftLimit = -500;

        public void Deposit(int amount)
        {
            _balance += amount;
            Console.WriteLine($"Deposited €{amount}, balance is now {_balance}");
        }

        public bool Withdraw(int amount)
        {
            if (_balance - amount >= _overdraftLimit)
            {
                _balance -= amount;
                Console.WriteLine($"Withdrew €{amount}, balance is now {_balance}");
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
    }

    public class BankAccountCommand : ICommand
    {
        public enum Action
        {
            Deposit, Withdraw
        }

        private BankAccount _account;
        private Action _action;
        private int _amount;
        private bool _succeeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            _account = account ?? throw new ArgumentNullException(nameof(account));
            _action = action;
            _amount = amount;
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

    class Program
    {
        static void Main()
        {
            var bankAccount = new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(bankAccount,BankAccountCommand.Action.Deposit,100),
                new BankAccountCommand(bankAccount,BankAccountCommand.Action.Withdraw,1000),

            };

            foreach (var command in commands)
            {
                command.Call();
            }

            Console.WriteLine(bankAccount);

            foreach (var command in Enumerable.Reverse(commands))
            {
                command.Undo();
            }
            Console.WriteLine(bankAccount);

        }
    }
}
