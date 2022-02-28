using System;

namespace Factory
{
    public enum CoordinatesSystem
    {
        Cartesian,
        Polar
    }

    public class Point
    {
        private double _x, _y;

        public Point(double x, double y, CoordinatesSystem system = CoordinatesSystem.Cartesian) // Is y or y rho in Polar? 
        {
            switch (system)
            {
                case CoordinatesSystem.Cartesian:
                    _x = x;
                    _y = y;
                    break;
                case CoordinatesSystem.Polar:
                    _x = x * Math.Cos(y);
                    _y = x * Math.Sin(y);
                    break;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
