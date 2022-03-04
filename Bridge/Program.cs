using System;
using Autofac;

namespace Bridge
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing pixels for circle with radius {radius}");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        private float _radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this._radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(_radius);
        }

        public override void Resize(float factor)
        {
            _radius *= factor;
        }
    }

    class Program
    {
        static void Main()
        {
            //IRenderer renderer = new RasterRenderer();
            //IRenderer renderer = new VectorRenderer();
            //var circle = new Circle(renderer, 5);
            //circle.Draw();
            //circle.Resize(2);
            //circle.Draw();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance();
            containerBuilder.Register(
                (context, parameter) =>
                    new Circle(
                        context.Resolve<IRenderer>(),
                        parameter.Positional<float>(0)
            ));

            using (var container = containerBuilder.Build())
            {
                var circle = container.Resolve<Circle>(new PositionalParameter(0, 5.0f));
                circle.Draw();
                circle.Resize(2.0f);
                circle.Draw();
            }
        }
    }
}
