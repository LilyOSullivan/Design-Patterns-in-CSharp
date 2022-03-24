using System;

namespace SwitchExpressions
{
    public enum Chest
    {
        Open,
        Closed,
        Locked
    }

    public enum Action
    {
        Open,
        Close
    }

    class Program
    {
        static Chest Manipulate(Chest chest, Action action, bool haveKey)
            =>
            (chest, action, haveKey) switch
            {
                (Chest.Locked, Action.Open, true) => Chest.Open,
                (Chest.Closed, Action.Open, _) => Chest.Open,
                (Chest.Open, Action.Close, true) => Chest.Locked,
                (Chest.Open, Action.Close, false) => Chest.Closed,
                _ => chest,
            };

        // For Transition actions
        static Chest Manipulate2(Chest chest,
      Action action, bool haveKey)
        {
            switch (chest, action, haveKey)
            {
                case (Chest.Closed, Action.Open, _):
                    return Chest.Open;
                case (Chest.Locked, Action.Open, true):
                    return Chest.Open;
                case (Chest.Open, Action.Close, true):
                    return Chest.Locked;
                case (Chest.Open, Action.Close, false):
                    return Chest.Closed;
                default:
                    Console.WriteLine("Chest unchanged");
                    return chest;
            }
        }

        static void Main()
        {
            var chest = Chest.Locked;
            Console.WriteLine($"Chest is {chest}");

            chest = Manipulate2(chest, Action.Open, true);
            Console.WriteLine($"Chest is {chest}");

            chest = Manipulate(chest, Action.Close, false);
            Console.WriteLine($"Chest is {chest}");

            chest = Manipulate(chest, Action.Close, false);
            Console.WriteLine($"Chest is {chest}");

        }
    }
}
