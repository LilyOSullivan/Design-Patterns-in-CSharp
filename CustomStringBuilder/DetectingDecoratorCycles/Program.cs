using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DetectingDecoratorCycles
{
    public interface IShape
    {
        public virtual string AsString() => string.Empty;
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

    public abstract class ShapeDecorator : IShape
    {
        protected internal readonly List<Type> _types = new();
        protected internal IShape _shape;

        protected ShapeDecorator(IShape shape)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));

            if (shape is ShapeDecorator shapeDecorator)
            {
                _types.AddRange(shapeDecorator._types);
            }
        }
    }

    public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
        where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
    {
        protected readonly TCyclePolicy policy = new();
        protected ShapeDecorator(IShape shape) : base(shape)
        {
            if (policy.TypeAdditionAllowed(typeof(TSelf), _types))
            {
                _types.Add(typeof(TSelf));
            }
        }
    }

    public class ShapedDecoratorWithPolicy<T> : ShapeDecorator<T, ThrowOnCyclePolicy>
    {
        public ShapedDecoratorWithPolicy(IShape shape) : base(shape)
        {
        }
    }

    public class ColoredShape
        : ShapeDecorator<ColoredShape, AbsorbCyclePolicy>
        //:ShapeDecoratorWithPolicy<ColoredShape>
    {
        private string _color;

        public ColoredShape(string color, IShape shape):base(shape)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));
            _color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public string AsString()
        {
            var stringBuilder = new StringBuilder($"{_shape.AsString()}");

            if (policy.ApplicationAllowed(_types[0], _types.Skip(1).ToList()))
                stringBuilder.Append($" has the color {_color}");

            return stringBuilder.ToString();
        }
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

    public abstract class ShapeDecoratorCyclePolicy
    {
        public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
        public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
    }

    public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
    {
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
            => true;

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
            => true;
    }

    public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
    {
        private bool handler(Type type, IList<Type> allTypes)
        {
            if (allTypes.Contains(type))
            {
                throw new InvalidOperationException(
                    $"Cycle Detected! Type is already a {type.FullName}"
                );
            }
            return true;
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return handler(type, allTypes);
        }

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return handler(type, allTypes);
        }
    }

    public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return true;
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return !allTypes.Contains(type);
        }
    }

    class Program
    {
        static void Main()
        {
            var circle = new Circle(2f);
            var redCircle = new ColoredShape("Red", circle);
            var coloredShape = new ColoredShape("blue", redCircle);

            Console.WriteLine(coloredShape.AsString());
        }
    }
}
