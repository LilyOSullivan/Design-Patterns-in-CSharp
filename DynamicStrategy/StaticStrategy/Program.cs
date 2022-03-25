using System;
using System.Collections.Generic;
using System.Text;

namespace StaticStrategy
{
    public enum OutputFormat
    {
        Markdown,
        Html
    }

    public interface IListStrategy
    {
        void Start(StringBuilder stringBuilder);
        void End(StringBuilder stringBuilder);
        void AddListItem(StringBuilder stringBuilder, string item);
    }

    public class MarkdownListStrategy : IListStrategy
    {
        public void Start(StringBuilder stringBuilder)
        {

        }

        public void End(StringBuilder stringBuilder)
        {

        }

        public void AddListItem(StringBuilder stringBuilder, string item)
        {
            stringBuilder.AppendLine($" * {item}");
        }
    }

    public class HtmlListStrategy : IListStrategy
    {
        public void Start(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("<ul>");
        }

        public void End(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("</ul>");
        }

        public void AddListItem(StringBuilder stringBuilder, string item)
        {
            stringBuilder.AppendLine($"  <li>{item}</li>");
        }
    }

    public class TextProcessor<LS> where LS : IListStrategy, new()
    {
        private StringBuilder stringBuilder = new StringBuilder();
        private IListStrategy listStrategy = new LS();

        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(stringBuilder);
            foreach (var item in items)
            {
                listStrategy.AddListItem(stringBuilder, item);
            }
            listStrategy.End(stringBuilder);
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
            var textProcessor = new TextProcessor<HtmlListStrategy>();
            textProcessor.AppendList(new[] { "Item 1!", "Item 2!!", "Item 3!!!" });
            Console.WriteLine(textProcessor.ToString());
        }
    }
}
