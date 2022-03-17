using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac;

namespace EventBroker
{
    public class Actor
    {
        protected EventBroker broker;

        public Actor(EventBroker broker)
        {
            this.broker = broker ?? throw new ArgumentNullException(nameof(broker));
        }
    }

    public class FootballPlayer : Actor
    {
        public string Name { get; set; }
        public int GoalsScored { get; set; } = 0;

        //public FootballPlayer(EventBroker broker) : base(broker)
        //{

        //}

        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            broker.OfType<PlayerScoredEvent>()
                .Where(playerEvent => !playerEvent.Name.Equals(name))
                .Subscribe(playerEvent =>
                {
                    Console.WriteLine($"{name}: Nicely done, {playerEvent.Name}. It's your {playerEvent.GoalsScored} goal.");
                }
            );

            broker.OfType<PlayerSentOffEvent>()
                .Where(playerEvent => !playerEvent.Name.Equals(name))
                .Subscribe(playerEvent =>
                {
                    Console.WriteLine($"{name}: Sent off :(, {playerEvent.Name}");
                }
            );
        }

        public void Score()
        {
            GoalsScored++;
            broker.Publish(new PlayerScoredEvent { Name = Name, GoalsScored = GoalsScored });
        }

        public void AssaultReferee()
        {
            broker.Publish(new PlayerSentOffEvent { Name = Name, Reason = "violence" });
        }
    }

    public class FootballCoach : Actor
    {
        public FootballCoach(EventBroker broker) : base(broker)
        {
            broker.OfType<PlayerScoredEvent>()
                .Subscribe(playerEvent =>
                {
                    if (playerEvent.GoalsScored < 3)
                    {
                        Console.WriteLine($"Coach: Well done, {playerEvent.Name}!");
                    }
                }
            );

            broker.OfType<PlayerSentOffEvent>()
                .Subscribe(playerEvent =>
                {
                    if (playerEvent.Reason == "violence")
                    {
                        Console.WriteLine($"Coach: How could you, {playerEvent.Name}.");
                    }
                }
            );
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class PlayerScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class PlayerSentOffEvent : PlayerEvent
    {
        public string Reason { get; set; }
    }

    public class EventBroker : IObservable<PlayerEvent>
    {
        private Subject<PlayerEvent> _subscriptions = new();

        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return _subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent playerEvent)
        {
            _subscriptions.OnNext(playerEvent);
        }
    }

    class Program
    {
        static void Main()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<EventBroker>().SingleInstance();
            containerBuilder.RegisterType<FootballCoach>();
            containerBuilder.Register((context, parameter) =>
                new FootballPlayer(
                    context.Resolve<EventBroker>(),
                    parameter.Named<string>("name")
                )
            );

            using (var container = containerBuilder.Build())
            {
                var coach = container.Resolve<FootballCoach>();
                var jane = container.Resolve<FootballPlayer>(new NamedParameter("name", "Jane"));
                var chris = container.Resolve<FootballPlayer>(new NamedParameter("name", "Chris"));

                jane.Score();
                jane.Score();
                jane.Score();

                chris.AssaultReferee();
            }

        }
    }
}
