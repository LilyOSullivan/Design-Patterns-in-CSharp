using System;

namespace Factory
{


    //public enum CoordinatesSystem
    //{
    //    Cartesian,
    //    Polar
    //}

    public class Point
    {
        // // Factory method
        //public static Point NewCartesianPoint(double x, double y)
        //{
        //    return new(x, y);
        //}

        //public static Point NewPolarPoint(double rho, double theta)
        //{
        //    return new(rho * Math.Cos(theta), rho * Math.Sin(theta));
        //}

        private double _x, _y;

        internal Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"x: {_x}, y: {_y}";
        }

        public static Point Origin => new Point(0, 0);
        public static Point Origin2 = new Point(0, 0);

        public static class Factory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            Console.WriteLine(point);
        }
    }
}
