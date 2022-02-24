using System;

namespace FluentBuilderInheritanceWithRecursiveGenerics
{
    public class Person
    {
        public string Name;
        public string Position;

        public class Builder : PersonJobBuilder<Builder>
        {

        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    public abstract class PersonBuilder
    {
        protected Person person = new Person();

        public Person Build()
        {
            return person;
        }
    }

    public class PersonInfoBuilder<Self> : PersonBuilder
        where Self : PersonInfoBuilder<Self>
    {
        public Self Called(string name)
        {
            person.Name = name;
            return (Self)this;
        }
    }

    public class PersonJobBuilder<Self> : PersonInfoBuilder<PersonJobBuilder<Self>>
        where Self : PersonJobBuilder<Self>
    {
        public Self WorksAsA(string position)
        {
            person.Position = position;
            return (Self)this;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var alex = Person.New.Called("Alex").WorksAsA("Chef").Build();
            Console.WriteLine(alex.ToString());
        }
    }
}
