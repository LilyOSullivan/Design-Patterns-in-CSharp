using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedText
{
    public class FormattedText
    {
        private readonly string _plainText;
        private bool[] capitalize;

        public FormattedText(string plainText)
        {
            _plainText = plainText;
            capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                capitalize[i] = true;
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < _plainText.Length; i++)
            {
                var c = _plainText[i];
                stringBuilder.Append(capitalize[i] ? char.ToUpper(c) : c);
            }
            return stringBuilder.ToString();
        }
    }

    public class BetterFormattedText
    {
        private string _plainText;
        private List<TextRange> formatting = new();

        public BetterFormattedText(string plainText)
        {
            _plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            formatting.Add(range);
            return range;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for(var i = 0; i < _plainText.Length; i++)
            {
                var c = _plainText[i];
                foreach(var range in formatting)
                {
                    if(range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        }

        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var formattedText = new FormattedText("This is a brave new World");
            formattedText.Capitalize(10, 15);
            Console.WriteLine(formattedText.ToString());

            var betterFormattedText = new BetterFormattedText("This is a brave new World");
            betterFormattedText.GetRange(10, 15).Capitalize = true;
            Console.WriteLine(betterFormattedText.ToString());
        }
    }
}
