using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace ObserverViaSpecialInterfaces
{
    public class Event
    {

    }

    public class FallsIllEvent : Event
    {
        public string Address;
    }

    public class Person : IObservable<Event>
    {
        private readonly HashSet<Subscription> _subscriptions = new();

        public IDisposable Subscribe(IObserver<Event> observer)
        {
            var subscription = new Subscription(this, observer);
            _subscriptions.Add(subscription);
            return subscription;
        }

        public void FallIll()
        {
            foreach (var s in _subscriptions)
            {
                s.Observer.OnNext(new FallsIllEvent { Address = "123 Paris Road" });
            }
        }

        private class Subscription : IDisposable
        {
            private Person _person { get; }
            public readonly IObserver<Event> Observer;
            public Subscription(Person person, IObserver<Event> observer)
            {
                _person = person;
                Observer = observer;
            }


            public void Dispose()
            {
                _person._subscriptions.Remove(this);
            }
        }
    }

    public class Program : IObserver<Event>
    {
        static void Main()
        {
            new Program();
        }

        public Program()
        {
            var person = new Person();
            //IDisposable subscription = person.Subscribe(this);

            person.OfType<FallsIllEvent>().Subscribe(
                args => Console.WriteLine($"A doctor required at {args.Address}")
            );

            person.FallIll();

        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Event value)
        {
            if (value is FallsIllEvent args)
            {
                Console.WriteLine($"A doctor required at {args.Address}");
            }
        }
    }
}
