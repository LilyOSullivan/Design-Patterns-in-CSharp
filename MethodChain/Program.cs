using System;

namespace MethodChain
{
    public class Creature
    {
        public string Name;
        public int Attack, Defence;

        public Creature(string name, int attack, int defence)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Attack = attack;
            Defence = defence;
        }

        public override string ToString()
        {
            return $"{Name} {Attack} {Defence}";
        }
    }

    public class CreatureModifier
    {
        protected Creature _creature;
        protected CreatureModifier next; // Linked-List like pattern

        public CreatureModifier(Creature creature)
        {
            _creature = creature;
        }

        public void Add(CreatureModifier creatureModifier)
        {
            if (next != null)
            {
                next.Add(creatureModifier);
            }
            else
            {
                next = creatureModifier;
            }
        }

        public virtual void Handle() => next?.Handle();
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine("Preventing Bonuses");
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Doubling {_creature.Name}'s attack!");
            _creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreaseDefenceModifier : CreatureModifier
    {
        public IncreaseDefenceModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {_creature.Name}'s defence!");
            _creature.Defence += 3;
            base.Handle();
        }
    }

    class Program
    {
        static void Main()
        {
            var goblin = new Creature("Goblin", 2, 10);
            Console.WriteLine(goblin);

            var root = new CreatureModifier(goblin);

            //root.Add(new NoBonusesModifier(goblin));
            root.Add(new DoubleAttackModifier(goblin));
            root.Add(new IncreaseDefenceModifier(goblin));

            root.Handle();

            Console.WriteLine(goblin);
        }
    }
}
