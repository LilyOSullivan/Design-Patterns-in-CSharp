using System;
using System.Collections.Generic;
using System.Linq;

namespace Iterator
{
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right) : this(value)
        {
            Left = left;
            Right = right;

            Left.Parent = Right.Parent = this;
        }
    }

    public class InOrderITerator<T>
    {
        private readonly Node<T> _root;
        public Node<T> Current { get; set; }
        private bool _yieldedStart;

        public InOrderITerator(Node<T> root)
        {
            _root = root;
            Current = root;

            while (Current.Left != null)
            {
                Current = Current.Left;
            }
        }

        public bool MoveNext()
        {
            if (!_yieldedStart)
            {
                _yieldedStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                {
                    Current = Current.Left;
                }
                return true;
            }
            else
            {
                var parent = Current.Parent;
                while (parent != null && Current == parent.Right)
                {
                    Current = parent;
                    parent = parent.Parent;
                }
                Current = parent;
                return Current != null;
            }
        }

        public void Reset()
        {
            Current = _root;
            _yieldedStart = false;
        }
    }

    public class BinaryTree<T>
    {
        private Node<T> _root;

        public BinaryTree(Node<T> root)
        {
            this._root = root;
        }

        public InOrderITerator<T> GetEnumerator()
        {
            return new InOrderITerator<T>(_root);
        }

        public IEnumerable<Node<T>> InOrder
        {
            get
            {
                IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    if (current.Left != null)
                    {
                        foreach (var left in Traverse(current.Left))
                        {
                            yield return left;
                        }
                    }
                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in Traverse(current.Right))
                        {
                            yield return right;
                        }
                    }
                }

                foreach (var node in Traverse(_root))
                {
                    yield return node;
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            //   1
            //  / \
            // 2   3

            // in-order: 213

            var root = new Node<int>(1, new Node<int>(2), new Node<int>(3));

            var iterator = new InOrderITerator<int>(root);
            while (iterator.MoveNext())
            {
                Console.Write(iterator.Current.Value);
                Console.Write(',');
            }
            Console.WriteLine();

            var tree = new BinaryTree<int>(root);
            Console.WriteLine(string.Join(",", tree.InOrder.Select(x => x.Value)));

            foreach(var node in tree)
            {
                Console.Write($"{node.Value},");
            }
        }
    }
}
