using System;

namespace MultipleInheritanceWithDefaultInterfaceMembers
{
    public interface ICreature
    {
        int Age { get; set; }
    }

    public interface IBird : ICreature
    {
        void Fly()
        {
            if (Age >= 10)
            {
                Console.WriteLine("Flying");
            }
        }
    }

    public interface ILizard : ICreature
    {
        void Crawl()
        {
            if (Age < 10)
            {
                Console.WriteLine("Crawling");
            }
        }
    }

    public class Organism
    {

    }

    public class Dragon : Organism, IBird, ILizard
    {
        public int Age { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var dragon = new Dragon { Age = 5 };
            //dragon.Fly();
            if (dragon is IBird bird)
            {
                bird.Fly();
            }
            if (dragon is ILizard lizard)
            {
                lizard.Crawl();
            }
        }
    }
}
