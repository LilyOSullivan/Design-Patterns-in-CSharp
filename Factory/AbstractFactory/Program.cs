using System;
using System.Collections.Generic;
using static System.Console;

namespace AbstractFactory
{
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This tea is nice but I would prefer it with milk");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This coffee is sensational!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int volume);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int volume)
        {
            WriteLine($"Put in a tea bag, boil water, pour {volume} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int volume)
        {
            WriteLine($"Grind some beans, boil water, pour {volume} ml, add cream and sugar and enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        private List<(string, IHotDrinkFactory)> factories = new();
        public HotDrinkMachine()
        {
            foreach (Type type in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(type) && !type.IsInterface)
                {
                    factories.Add((
                        type.Name.Replace("Factory", string.Empty),
                        (IHotDrinkFactory)Activator.CreateInstance(type))
                    );
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            WriteLine("Available drinks");
            for (int i = 0; i < factories.Count; i++)
            {
                var tuple = factories[i];
                WriteLine($"{i}: {tuple.Item1}");
            }
            while (true)
            {
                string s;
                if (
                    (s = ReadLine()) != null &&
                    int.TryParse(s, out int i) &&
                    i >= 0 &&
                    i < factories.Count
                )
                {
                    Write("Specify amount: ");
                    s = ReadLine();
                    if (s != null && int.TryParse(s, out int amount) && amount > 0)
                    {
                        return factories[i].Item2.Prepare(amount);
                    }
                }
                WriteLine("Incorrect input, try again");
            }
        }

        //public enum AvailableDrink
        //{
        //    Coffee, Tea
        //}

        //private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new();

        //public HotDrinkMachine()
        //{
        //    foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
        //    {
        //        var factory = (IHotDrinkFactory)Activator.CreateInstance(
        //            Type.GetType("AbstractFactory." +
        //            Enum.GetName(
        //                typeof(AvailableDrink),
        //                drink) + "Factory"
        //            )
        //        );
        //        factories.Add(drink, factory);
        //    }
        //}

        //public IHotDrink MakeDrink(AvailableDrink drink,int amount)
        //{
        //    return factories[drink].Prepare(amount);
        //}
    }

    class Program
    {
        static void Main()
        {
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}
