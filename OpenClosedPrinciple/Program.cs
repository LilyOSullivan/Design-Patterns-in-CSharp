using System;
using System.Collections.Generic;

namespace OpenClosedPrinciple
{
    class Program
    {
        public enum Color
        {
            Red, Green, Blue
        }

        public enum Size
        {
            Small, Medium, Large, Huge
        }

        public class Product
        {
            public string Name;
            public Color Color;
            public Size Size;

            public Product(string name, Color color, Size size)
            {
                this.Name = name ?? throw new ArgumentNullException(nameof(name));
                this.Color = color;
                this.Size = size;
            }
        }

        public class ProductFilter
        {
            public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
            {
                foreach (Product p in products)
                {
                    if (p.Size == size)
                        yield return p;
                }
            }

            public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Color color)
            {
                foreach (Product p in products)
                {
                    if (p.Color == color)
                        yield return p;
                }
            }

            public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Color color, Size size)
            {
                foreach (Product p in products)
                {
                    if (p.Color == color && p.Size == size)
                        yield return p;
                }
            }
        }

        public interface ISpecification<T>
        {
            bool IsSatisfied(T t);
        }

        public interface IFilter<T>
        {
            IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> specification);
        }

        public class ColorSpecification : ISpecification<Product>
        {
            private Color _color;

            public ColorSpecification(Color color)
            {
                this._color = color;
            }

            public bool IsSatisfied(Product t)
            {
                return t.Color == _color;
            }
        }

        public class AndSpecification<T> : ISpecification<T>
        {
            private ISpecification<T> _first, _second;

            public AndSpecification(ISpecification<T> first, ISpecification<T> second)
            {
                this._first = first ?? throw new ArgumentException(nameof(first));
                this._second = second ?? throw new ArgumentException(nameof(second));
            }

            public bool IsSatisfied(T t)
            {
                return _first.IsSatisfied(t) && _second.IsSatisfied(t);
            }
        }

        public class BetterFilter : IFilter<Product>
        {
            public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> specification)
            {
                foreach (Product product in items)
                {
                    if (specification.IsSatisfied(product))
                    {
                        yield return product;
                    }
                }
            }
        }

        public class SizeSpecification : ISpecification<Product>
        {
            private Size _size;
            public SizeSpecification(Size size)
            {
                this._size = size;
            }

            public bool IsSatisfied(Product p)
            {
                return p.Size == _size;
            }
        }

        static void Main()
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Huge);
            Product[] products = { apple, tree, house };


            var productFilter = new ProductFilter();
            Console.WriteLine("Green products (old):");
            foreach (var product in productFilter.FilterBySize(products, Color.Green))
            {
                Console.WriteLine($" - {product.Name} is green");
            }


            // Implementing Open-Close
            var betterFilter = new BetterFilter();
            Console.WriteLine("Green products (new):");
            foreach (var product in betterFilter.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {product.Name} is green");
            }

            Console.WriteLine("Huge blue items");
            foreach (var product in betterFilter.Filter(
                products,
                new AndSpecification<Product>(new ColorSpecification(Color.Blue),
                                              new SizeSpecification(Size.Huge))))
            {
                Console.WriteLine($" - {product.Name} is huge and blue");
            }
        }
    }
}
