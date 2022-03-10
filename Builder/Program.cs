using System;
using System.Collections.Generic;
using System.Text;

namespace Builder
{
    public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int _indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        private string ToStringImpl(int indent)
        {
            var stringBuilder = new StringBuilder();
            var i = new string(' ', _indentSize * indent);
            stringBuilder.Append($"{i}<{Name}>\n");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                stringBuilder.Append(new string(' ', _indentSize * (indent + 1)));
                stringBuilder.AppendLine(Text);
            }

            foreach (var e in Elements)
            {
                stringBuilder.Append(e.ToStringImpl(indent + 1));
            }
            stringBuilder.Append($"{i}</{Name}>\n");
            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder
    {
        HtmlElement root = new HtmlElement();
        private readonly string _rootName;

        public HtmlBuilder(string rootName)
        {
            this._rootName = rootName;
            root.Name = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var element = new HtmlElement(childName, childText);
            root.Elements.Add(element);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement { Name = _rootName };
        }

    }

    class Program
    {
        static void Main()
        {
            var hello = "hello";
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<p>");
            stringBuilder.Append(hello);
            stringBuilder.Append("</p>");
            Console.WriteLine(stringBuilder.ToString());

            var words = new[] { "hello", "world" };
            stringBuilder.Clear();
            stringBuilder.Append("<ul>");
            foreach (string word in words)
            {
                stringBuilder.AppendFormat("<li>{0}</li>", word);
            }
            stringBuilder.Append("</ul>");
            Console.WriteLine(stringBuilder.ToString());

            Console.WriteLine();

            // Ordinary non-fluent builder
            var builder = new HtmlBuilder("ul");
            builder.AddChild("li", "hello");
            builder.AddChild("li", "world");
            Console.WriteLine(builder.ToString());

            // Fluent interface
            var builderFluent = new HtmlBuilder("ul");
            builder.AddChild("li", "hello").AddChild("li", "world");
            Console.WriteLine(builder.ToString());
        }
    }
}
