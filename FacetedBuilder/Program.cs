using System;

namespace FacetedBuilder
{
    public class Person
    {
        public string StreetAddress, Postcode, City;

        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
            => $"{StreetAddress}, {Postcode}, {City}, {CompanyName}, {Position}, {AnnualIncome}";
    }

    public class PersonBuilder // Facade
    {
        protected Person person = new();

        public PersonAddressBuilder Lives => new(person);
        public PersonJobBuilder Works => new(person);

        public static implicit operator Person(PersonBuilder pb)
        {
            return pb.person;
        }
    }

    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var personBuilder = new PersonBuilder();
            Person person = personBuilder
              .Lives
                .At("40 Main Street")
                .In("Dublin")
                .WithPostcode("L21 P6G4")
              .Works
                .At("Apple")
                .AsA("Software Engineer")
                .Earning(123_000);
        }
    }
}
