using System;
using System.Linq;

namespace ArrayBackedProperties
{
    public class Creature
    {
        private int[] stats = new int[3];

        public int Strength
        {
            get => stats[0];
            set => stats[0] = value;
        }
        public int Agility
        {
            get => stats[1];
            set => stats[1] = value;
        }
        public int Intelligence
        {
            get => stats[2];
            set => stats[2] = value;
        }

        public int this[int index]
        {
            get => stats[index];
            set => stats[index] = value;
        }

        public double AverageStat => stats.Average();
    }

    class Program
    {
        static void Main()
        {

        }
    }
}
