using System;

namespace MultipleInheritanceWithInterfaces
{

    public interface IBird
    {
        int Weight { get; set; }
        void Fly();

    }

    public class Bird : IBird
    {
        public int Weight { get; set; }

        public void Fly()
        {
            Console.WriteLine($"Bird Flying with weight {Weight}");
        }
    }

    public interface ILizard
    {
        int Weight { get; set; }
        void Crawl();
    }

    public class Lizard : ILizard
    {
        public int Weight { get; set; }

        public void Crawl()
        {
            Console.WriteLine($"Lizard Crawling with weight {Weight}");
        }
    }

    public class Dragon : IBird, ILizard
    {
        private Bird _bird = new();
        private Lizard _lizard = new();

        private int _weight;

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; _bird.Weight = value; _lizard.Weight = value; }
        }

        //public Dragon(Bird bird, Lizard lizard)
        //{
        //    _bird = bird ?? throw new ArgumentNullException(nameof(bird));
        //    _lizard = lizard ?? throw new ArgumentNullException(nameof(lizard));
        //}

        public void Crawl()
        {
            _lizard.Crawl();
        }

        public void Fly()
        {
            _bird.Fly();
        }
    }

    class Program
    {
        static void Main()
        {
            var dragon = new Dragon();
            dragon.Weight = 123;
            dragon.Fly();
            dragon.Crawl();
        }
    }
}
