using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BidirectionalObserver
{
    public class Product : INotifyPropertyChanged
    {
        private string _name { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return $"Product: {Name}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Window : INotifyPropertyChanged
    {
        private string productName;

        public string ProductName
        {
            get => productName;
            set
            {
                if (value == productName) return; // Prevents infinite recursion
                productName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Window()
        {

        }

        public Window(Product product)
        {
            ProductName = product.Name;
        }

        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Window: {ProductName}";
        }
    }

    public sealed class BidirectionalBinding : IDisposable
    {
        private bool _disposed;

        public BidirectionalBinding(
            INotifyPropertyChanged first,
            Expression<Func<object>> firstProperty,
            INotifyPropertyChanged second,
            Expression<Func<object>> secondProperty
        )
        {
            if (firstProperty.Body is MemberExpression firstExpr &&
                secondProperty.Body is MemberExpression secondExpr)
            {
                if (firstExpr.Member is PropertyInfo firstProp
                    && secondExpr.Member is PropertyInfo secondProp)
                {
                    first.PropertyChanged += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            secondProp.SetValue(second, firstProp.GetValue(first));
                        }
                    };

                    second.PropertyChanged += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            firstProp.SetValue(first, secondProp.GetValue(second));
                        }
                    };
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }

    class Program
    {
        static void Main()
        {
            var product = new Product { Name = "Book" };
            var window = new Window { ProductName = "Book" };

            using var binding = new BidirectionalBinding(
                product,
                () => product.Name,
                window,
                () => window.ProductName
            );

            product.Name = "Smart Book";
            window.ProductName = "Really Smart Book";
            Console.WriteLine(product);
            Console.WriteLine(window);
        }
    }
}
