using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace RepeatingUserNames
{
    public class User
    {
        private string _fullName;

        public User(string fullName)
        {
            _fullName = fullName;
        }
    }

    public class User2
    {
        static List<string> strings = new();
        private int[] names;

        public string FullName => string.Join(" ", names.Select(i => strings[i]));

        public User2(string fullName)
        {
            int getOrAdd(string s)
            {
                int i = strings.IndexOf(s);
                if (i != -1)
                {
                    return i;
                }
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }
            names = fullName.Split(' ').Select(getOrAdd).ToArray();
        }
    }

    [TestFixture]
    class Program
    {
        static void Main()
        {

        }

        [Test]
        public void TestUser()
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();

            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User($"{firstNames} {lastNames}"));
                }
            }

            ForceGC();

            dotMemory.Check(memory =>
            {
                Console.WriteLine(memory.SizeInBytes);
            });
        }

        public void TestUser2()
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User2>();

            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User2($"{firstNames} {lastNames}"));
                }
            }

            ForceGC();

            dotMemory.Check(memory =>
            {
                Console.WriteLine(memory.SizeInBytes);
            });
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private string RandomString()
        {
            Random rand = new Random();
            return new string(
                Enumerable.Range(0, 10)
                .Select(i => (char)('a' + rand.Next(26)))
                .ToArray()
            );
        }
    }
}
