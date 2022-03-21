using System;
using System.Collections.Generic;

namespace Memento
{
    public class Memento
    {
        public int Balance { get; }

        public Memento(int balance)
        {
            Balance = balance;
        }
    }

    public class BankAccount
    {
        private int _balance;
        private List<Memento> _changes = new();
        private int _current;

        public BankAccount(int balance)
        {
            this._balance = balance;
            _changes.Add(new Memento(balance));
        }

        public Memento Deposit(int amount)
        {
            _balance += amount;
            var memento = new Memento(_balance);
            _changes.Add(memento);
            ++_current;
            return memento;
        }

        public Memento Restore(Memento memento)
        {
            if (memento != null)
            {
                _balance = memento.Balance;
                _changes.Add(memento);
                return memento;
            }
            return null;
        }

        public Memento Undo()
        {
            if (_current > 0)
            {
                var memento = _changes[--_current];
                _balance = memento.Balance;
                return memento;
            }
            return null;
        }

        public Memento Redo()
        {
            if (_current + 1 < _changes.Count)
            {
                var memento = _changes[++_current];
                _balance = memento.Balance;
                return memento;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{nameof(_balance)}: {_balance}";
        }
    }

    class Program
    {
        static void Main()
        {
            var bankAccount = new BankAccount(100);
            bankAccount.Deposit(50); // Balance: 150
            bankAccount.Deposit(25); // Balance: 175

            Console.WriteLine(bankAccount);
            bankAccount.Undo();
            Console.WriteLine($"Undo: {bankAccount}");
            bankAccount.Undo();
            Console.WriteLine($"Undo: {bankAccount}");
            bankAccount.Redo();
            Console.WriteLine($"Redo: {bankAccount}");
        }
    }
}
