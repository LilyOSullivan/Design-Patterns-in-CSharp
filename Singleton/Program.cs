using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using Autofac;

namespace Singleton
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;
        private static int _instanceCount;
        public static int Count => _instanceCount;

        private SingletonDatabase()
        {
            _instanceCount++;
            Console.WriteLine("Initializing database");
            capitals = File.ReadAllLines("capitals.txt")
                //File.ReadAllLines(Path.Combine(
                //    new FileInfo(
                //        (typeof(IDatabase).Assembly.Location)+"capitals.txt").DirectoryName
                //    )
                //)
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        private static Lazy<SingletonDatabase> _instance = new(() => new SingletonDatabase());
        public static SingletonDatabase Instance => _instance.Value;

    }

    public class OrdinaryDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        public OrdinaryDatabase()
        {
            Console.WriteLine("Initializing database");
            capitals = File.ReadAllLines("capitals.txt")
                //File.ReadAllLines(Path.Combine(
                //    new FileInfo(
                //        (typeof(IDatabase).Assembly.Location)+"capitals.txt").DirectoryName
                //    )
                //)
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }

    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
            {
                result += SingletonDatabase.Instance.GetPopulation(name);
            }
            return result;
        }
    }

    public class ConfigureableRecordFinder
    {
        private IDatabase _database;
        public ConfigureableRecordFinder(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
            {
                result += _database.GetPopulation(name);
            }
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int> { ["alpha"] = 1, ["beta"] = 2, ["gamma"] = 3 }[name];
        }
    }

    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;

            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalPopulationTest()
        {
            var recordFinder = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            var totalPopulation = recordFinder.GetTotalPopulation(names);
            Assert.That(totalPopulation, Is.EqualTo(17500000 + 17400000));
        }

        [Test]
        public void ConfigurablePopulationTest()
        {
            var configurableRecordFinder = new ConfigureableRecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };
            int totalPopulation = configurableRecordFinder.GetTotalPopulation(names);
            Assert.That(totalPopulation, Is.EqualTo(4));
        }

        [Test]
        public void DependencyInjectionPopulationTest()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<OrdinaryDatabase>()
                .As<IDatabase>()
                .SingleInstance();
            containerBuilder.RegisterType<ConfigureableRecordFinder>();
            using (var c = containerBuilder.Build())
            {
                var recordFinder = c.Resolve<ConfigureableRecordFinder>();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var db = SingletonDatabase.Instance;
            var city = "Tokyo";
            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");
        }
    }
}
