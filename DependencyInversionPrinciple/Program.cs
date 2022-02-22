using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInversionPrinciple
{
    public enum Relationship
    {
        Parent, Child, Sibling
    }

    public class Person
    {
        public string Name;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    public class Relationships : IRelationshipBrowser
    {
        //public List<(Person, Relationship, Person)> Relations => _relations;

        private List<(Person, Relationship, Person)> _relations = new();

        public void AddParentAndChild(Person parent, Person child)
        {
            var newEntry = (parent, Relationship.Parent, child);
            _relations.Add(newEntry);

            newEntry = (child, Relationship.Child, parent);
            _relations.Add(newEntry);
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return _relations.Where(
                x => x.Item1.Name == name &&
                     x.Item2 == Relationship.Parent
            ).Select(relation => relation.Item3);
        }
    }

    class Research
    {
        //public Research(Relationships relationships)
        //{
        //    var relations = relationships.Relations;
        //    foreach (var r in relations.Where(x => x.Item1.Name == "Alex"
        //                                           && x.Item2 == Relationship.Parent))
        //    {
        //        Console.WriteLine($"Alex has a child called {r.Item3.Name}");
        //    }
        //}

        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("Alex"))
            {
                Console.WriteLine($"Alex has a child called {p.Name}");
            }
        }

        static void Main()
        {
            var parent = new Person { Name = "Alex" };
            var child1 = new Person { Name = "Andie" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
}
