using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicStrategy
{
    public enum OutputFormat
    {
        Markdown,
        Html,
    }

    public interface IListStrategy
    {
        void Start(StringBuilder stringBuilder);
        void End(StringBuilder stringBuilder);
        void AddListItem(StringBuilder stringBuilder, string item);
    }

    public class HtmlListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder stringBuilder, string item)
        {
            stringBuilder.AppendLine($"  <li>{item}</li>");
        }

        public void End(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("</ul>");

        }

        public void Start(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("<ul>");
        }
    }

    public class MarkdownListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder stringBuilder, string item)
        {
            stringBuilder.AppendLine($" * {item}");
        }

        public void End(StringBuilder stringBuilder)
        {

        }

        public void Start(StringBuilder stringBuilder)
        {

        }
    }

    public class TextProcessor
    {
        private StringBuilder stringBuilder = new();
        private IListStrategy listStrategy;

        public void SetOutputFormat(OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.Markdown:
                    listStrategy = new MarkdownListStrategy();
                    break;
                case OutputFormat.Html:
                    listStrategy = new HtmlListStrategy();
                    break;
            }
        }

        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(stringBuilder);
            foreach (var item in items)
            {
                listStrategy.AddListItem(stringBuilder, item);
            }
            listStrategy.End(stringBuilder);
        }

        public StringBuilder Clear()
        {
            return stringBuilder.Clear();
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }

    class Program
    {
        static void Main()
        {
            var textProcessor = new TextProcessor();
            textProcessor.SetOutputFormat(OutputFormat.Html);
            textProcessor.AppendList(new[] { "Item 1!", "Item 2!!", "Item 3!!!" });
            Console.WriteLine(textProcessor.ToString());
        }
    }
}
