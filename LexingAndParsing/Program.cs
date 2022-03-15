using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LexingAndParsing
{
    public interface IElement
    {
        int Value { get; }
    }

    public class Integer : IElement
    {
        public int Value { get; }

        public Integer(int value)
        {
            Value = value;
        }
    }

    public class BinaryOperation : IElement
    {
        public enum Type
        {
            Addition, Subtraction
        }

        public Type MyType;
        public IElement Left, Right;


        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Subtraction:
                        return Left.Value - Right.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public class Token
    {
        public enum Type
        {
            Integer, Plus, Minus, OpenBracket, CloseBracket
        }

        public Type MyType;
        public string Text;

        public Token(Type myType, string text)
        {
            MyType = myType;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override string ToString()
        {
            return $"`{Text}`";
        }
    }

    class Program
    {
        static List<Token> Lexer(string input)
        {
            var result = new List<Token>();
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        result.Add(new(Token.Type.Plus, "+"));
                        break;
                    case '-':
                        result.Add(new(Token.Type.Minus, "-"));
                        break;
                    case '(':
                        result.Add(new(Token.Type.OpenBracket, "("));
                        break;
                    case ')':
                        result.Add(new(Token.Type.CloseBracket, ")"));
                        break;
                    default:
                        var stringBuilder = new StringBuilder(input[i].ToString());
                        for (int j = i + 1; j < input.Length; j++)
                        {
                            if (char.IsDigit(input[j]))
                            {
                                stringBuilder.Append(input[j]);
                                ++i;
                            }
                            else
                            {
                                result.Add(new(Token.Type.Integer, stringBuilder.ToString()));
                                break;
                            }
                        }
                        break;
                }
            }

            return result;
        }

        static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOperation();
            bool haveLeftHandSide = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        var integer = new Integer(int.Parse(token.Text));
                        if (!haveLeftHandSide)
                        {
                            result.Left = integer;
                            haveLeftHandSide = true;
                        }
                        else
                        {
                            result.Right = integer;
                        }
                        break;
                    case Token.Type.Plus:
                        result.MyType = BinaryOperation.Type.Addition;
                        break;
                    case Token.Type.Minus:
                        result.MyType = BinaryOperation.Type.Subtraction;
                        break;
                    case Token.Type.OpenBracket:
                        int j = i;
                        for (; j < tokens.Count; ++j)
                        {
                            if (tokens[j].MyType == Token.Type.CloseBracket)
                            {
                                break;
                            }
                        }
                        var subExpression = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                        var element = Parse(subExpression);
                        if (!haveLeftHandSide)
                        {
                            result.Left = element;
                            haveLeftHandSide = true;
                        }
                        else
                        {
                            result.Right = element;
                        }
                        i = j;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }

        static void Main()
        {
            string input = "(13+4)-(12+1)";
            var tokens = Lexer(input);
            Console.WriteLine(string.Join("\t", tokens));

            var parsed = Parse(tokens);
            Console.WriteLine($"{input} = {parsed.Value}");
        }
    }
}
