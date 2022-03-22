using System;

namespace Observer
{
    public class FallsIllEventArgs
    {
        public string Address;
    }

    public class Person
    {
        public event EventHandler<FallsIllEventArgs> FallsIll;

        public void CatchCold()
        {
            FallsIll?.Invoke(this, new FallsIllEventArgs { Address = "123 Paris Road" });
        }
    }

    class Program
    {
        static void Main()
        {
            var person = new Person();
            person.FallsIll += CallDoctor;
            person.CatchCold();
        }

        private static void CallDoctor(object sender, FallsIllEventArgs e)
        {
            Console.WriteLine($"A doctor has been called to {e.Address}");
        }
    }
}
