using System;

namespace BrokerChain
{
    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query query)
        {
            Queries?.Invoke(sender, query);
        }
    }

    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack, Defence
        }

        public Argument WhatToQuery;

        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Creature
    {
        private Game game;
        public string Name;
        private int attack, defence;

        public Creature(Game game, string name, int attack, int defence)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.attack = attack;
            this.defence = defence;
        }

        public int Attack
        {
            get
            {
                var query = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, query);
                return query.Value;
            }
        }

        public int Defence
        {
            get
            {
                var query = new Query(Name, Query.Argument.Defence, defence);
                game.PerformQuery(this, query);
                return query.Value;
            }
        }

        public override string ToString()
        {
            return $"{Name} {Attack} {Defence}";
        }
    }

    public abstract class CreatureModifier : IDisposable
    {
        protected Game _game;
        protected Creature _creature;

        protected CreatureModifier(Game game, Creature creature)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _creature = creature ?? throw new ArgumentNullException(nameof(creature));
            _game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query query);

        public void Dispose()
        {
            _game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query query)
        {
            if (query.CreatureName == _creature.Name
                && query.WhatToQuery == Query.Argument.Attack
            )
            {
                query.Value *= 2;
            }
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query query)
        {
            if (query.CreatureName == _creature.Name
                && query.WhatToQuery == Query.Argument.Defence
            )
            {
                query.Value += 3;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var game = new Game();
            var goblin = new Creature(game, "Strong Goblin", 2, 10);
            Console.WriteLine(goblin);
            using (new DoubleAttackModifier(game, goblin))
            {
                Console.WriteLine(goblin);
                using (new IncreaseDefenseModifier(game, goblin))
                {
                    Console.WriteLine(goblin);
                }
            }
            Console.WriteLine(goblin);

        }
    }
}
