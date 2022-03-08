using System;

namespace DynamicDecoratorComposition
{
    public interface IShape
    {
        string AsString();
    }

    public class Circle : IShape
    {
        private float _radius;

        public Circle(float radius)
        {
            _radius = radius;
        }

        public void Resize(float factor)
        {
            _radius *= factor;
        }

        public string AsString() => $"A circle with radius {_radius}";
    }

    public class Square : IShape
    {
        private float _side;

        public Square(float side)
        {
            _side = side;
        }

        public string AsString() => $"A square with side {_side}";
    }

    public class ColoredShape : IShape
    {
        private IShape _shape;
        private string _color;

        public ColoredShape(string color, IShape shape)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));
            _color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public string AsString() => $"{_shape.AsString()} has the color {_color}";
    }

    public class TransparentShape : IShape
    {
        private IShape _shape;
        private float _transparency;

        public TransparentShape(float transparency, IShape shape)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));
            _transparency = transparency;
        }

        public string AsString() => $"{_shape.AsString()} has {_transparency * 100.0}% transparency";
    }

    class Program
    {
        static void Main()
        {
            var square = new Square(1.23f);
            Console.WriteLine(square.AsString());

            var redSquare = new ColoredShape("Red", new Square(1.5f));
            Console.WriteLine(redSquare.AsString());

            var redHalfTransparentSquare = new TransparentShape(0.5f, redSquare);
            Console.WriteLine(redHalfTransparentSquare.AsString());
        }
    }
}
