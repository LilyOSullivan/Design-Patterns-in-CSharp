using System;
using System.Collections.Generic;
using System.Text;

namespace ReflectiveVisitor
{
    using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

    public abstract class Expression
    {

    }

    public class DoubleExpression : Expression
    {
        public double Value;
        public DoubleExpression(double value)
        {
            this.Value = value;
        }

    }

    public class AdditionExpression : Expression
    {
        public Expression Left, Right;

        public AdditionExpression(Expression left, Expression right)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
        }
    }

    public static class ExpressionPrinter
    {
        private static DictType actions = new DictType
        {
            [typeof(DoubleExpression)] = (expression, stringBuilder) =>
            {
                var doubleExpression = (DoubleExpression)expression;
                stringBuilder.Append(doubleExpression.Value);
            },
            [typeof(AdditionExpression)] = (expression, stringBuilder) =>
            {
                var additionExpression = (AdditionExpression)expression;
                stringBuilder.Append('(');
                Print(additionExpression.Left, stringBuilder);
                stringBuilder.Append('+');
                Print(additionExpression.Right, stringBuilder);
                stringBuilder.Append(')');
            },
        };

        public static void Print(Expression expression, StringBuilder stringBuilder)
        {
            actions[expression.GetType()](expression, stringBuilder);
        }

        //public static void Print(Expression expression, StringBuilder stringBuilder)
        //{
        //    if (expression is DoubleExpression doubleExpression)
        //    {
        //        stringBuilder.Append(doubleExpression.Value);
        //    }
        //    else if (expression is AdditionExpression additionExpression)
        //    {
        //        stringBuilder.Append('(');
        //        Print(additionExpression.Left, stringBuilder);
        //        stringBuilder.Append('+');
        //        Print(additionExpression.Right, stringBuilder);
        //        stringBuilder.Append(')');
        //    }
        //}
    }

    class Program
    {
        static void Main()
        {
            var additionExpression = new AdditionExpression(
                new DoubleExpression(1),
                new AdditionExpression(
                    new DoubleExpression(2),
                    new DoubleExpression(3)
                )
            );
            var stringBuilder = new StringBuilder();
            ExpressionPrinter.Print(additionExpression, stringBuilder);
            Console.WriteLine(stringBuilder);
        }
    }
}
