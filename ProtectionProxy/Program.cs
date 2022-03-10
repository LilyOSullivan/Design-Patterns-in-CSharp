using System;

namespace ProtectionProxy
{
    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("Car is being driven");
        }
    }

    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }

    public class CarProxy : ICar
    {
        private Driver _driver;
        private Car _car = new();

        public CarProxy(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public void Drive()
        {
            if (_driver.Age >= 16)
            {
                _car.Drive();
            }
            else
            {
                Console.WriteLine("Too young");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            ICar car = new CarProxy(new Driver(16));
            car.Drive();
        }
    }
}
