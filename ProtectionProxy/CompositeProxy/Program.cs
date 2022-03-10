using System;
using System.Collections.Generic;

namespace CompositeProxy
{
    class Creature
    {
        public byte age;
        public int X, Y;
    }

    class Creatures
    {
        private readonly int _size;
        private byte[] _age;
        private int[] _x, _y;

        public Creatures(int size)
        {
            this._size = size;
            _age = new byte[size];
            _x = new int[size];
            _y = new int[size];
        }

        public struct CreatureProxy
        {
            private readonly Creatures creatures;
            private readonly int index;

            public CreatureProxy(Creatures creatures, int index)
            {
                this.creatures = creatures;
                this.index = index;
            }

            public ref byte Age => ref creatures._age[index];
            public ref int X => ref creatures._x[index];
            public ref int Y => ref creatures._y[index];

        }

        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int pos = 0; pos < _size; ++pos)
            {
                yield return new CreatureProxy(this, pos);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // AoS; Array of Structures
            var creatures = new Creature[100];
            foreach (var c in creatures)
            {
                c.X++;
            }

            // SoA; Structure of Arrays
            var creatures2 = new Creatures(100);
            foreach (var c in creatures2)
            {
                c.X++;
            }


        }
    }
}
