using System;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color;

        private Lazy<List<GraphicObject>> children = new();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder stringBuilder, int depth)
        {
            stringBuilder.Append(new string('*', depth))
                .Append(
                    string.IsNullOrWhiteSpace(Color) ?
                    string.Empty :
                    $"{Color} "
                ).AppendLine(Name);
            foreach (var child in Children)
            {
                child.Print(stringBuilder, depth + 1);
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            Print(stringBuilder, 0);
            return stringBuilder.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }

    class Program
    {
        static void Main()
        {
            var drawing = new GraphicObject { Name = "My Drawing" };
            drawing.Children.Add(new Square { Color = "Red" });
            drawing.Children.Add(new Circle { Color = "Yellow" });

            var group = new GraphicObject();
            group.Children.Add(new Circle { Color = "Blue" });
            group.Children.Add(new Square { Color = "Blue" });
            drawing.Children.Add(group);

            Console.WriteLine(drawing);
        }
    }
}
