using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace PrototypeInheritance
{

    //public static class ExtensionMethodss
    //{
    //    public static T DeepCopy<T>(this T self)
    //    {
    //        var stream = new MemoryStream();
    //        var formatter = new BinaryFormatter();
    //        formatter.Serialize(stream, self);
    //        stream.Seek(0, SeekOrigin.Begin);
    //        object copy = formatter.Deserialize(stream);
    //        stream.Close();
    //        return (T)copy;
    //    }

    //    public static T DeepCopyXml<T>(this T self)
    //    {
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            var s = new XmlSerializer(typeof(T));
    //            s.Serialize(memoryStream, self);
    //            memoryStream.Position = 0;
    //            return (T)s.Deserialize(memoryStream);
    //        }
    //    }
    //}

    public interface IDeepCopyable<T>
        where T : new()
    {
        void CopyTo(T target);
        public T DeepCopy()
        {
            T t = new();
            CopyTo(t);
            return t;
        }
    }

    //[Serializable]
    public class Address : IDeepCopyable<Address>
    {
        public string StreetName;
        public int HouseNumber;

        public Address()
        {

        }

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName ?? throw new ArgumentNullException(nameof(streetName));
            HouseNumber = houseNumber;
        }

        public void CopyTo(Address target)
        {
            target.StreetName = StreetName;
            target.HouseNumber = HouseNumber;
        }

        public override string ToString()
        {
            return $"{StreetName}, {HouseNumber}";
        }
    }

    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this IDeepCopyable<T> item)
            where T : new()
        {
            return item.DeepCopy();
        }

        public static T DeepCopy<T>(this T person)
            where T : Person, new()
        {
            return ((IDeepCopyable<T>)person).DeepCopy();
        }
    }

    //[Serializable]
    public class Person : IDeepCopyable<Person>
    {
        public string[] Names;
        public Address Address;

        public Person()
        {

        }

        public Person(string[] names, Address address)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
            Address = address ?? throw new ArgumentNullException(nameof(names));
        }

        public void CopyTo(Person target)
        {
            target.Names = (string[])Names.Clone();
            target.Address = Address.DeepCopy();
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Names)}, {Address}";
        }
    }

    //[Serializable]
    public class Employee : Person, IDeepCopyable<Employee>
    {
        public int Salary;

        public Employee()
        {

        }

        public Employee(string[] names, Address address, int salary)
            : base(names, address)
        {
            Salary = salary;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Salary}";
        }

        public void CopyTo(Employee target)
        {
            base.CopyTo(target);
            target.Salary = Salary;
        }
    }

    class Program
    {
        static void Main()
        {
            var john = new Employee();
            john.Names = new[] { "John", "Doe" };
            john.Address = new Address { HouseNumber = 123, StreetName = "London Road" };
            john.Salary = 321_000;

            var jane = john.DeepCopy();
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber++;
            jane.Salary++;

            Console.WriteLine(john);
            Console.WriteLine(jane);

        }
    }
}
