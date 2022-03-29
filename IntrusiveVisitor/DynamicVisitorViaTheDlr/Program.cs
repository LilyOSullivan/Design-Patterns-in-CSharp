using System;
using System.Text;

namespace DynamicVisitorViaTheDlr
{
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

    public class ExpressionPrinter
    {
        public void Print(AdditionExpression additionExpression, StringBuilder stringBuilder)
        {
            stringBuilder.Append('(');
            Print((dynamic)additionExpression.Left, stringBuilder);
            stringBuilder.Append('+');
            Print((dynamic)additionExpression.Right, stringBuilder);
            stringBuilder.Append(')');

        }

        public void Print(DoubleExpression doubleExpression, StringBuilder stringBuilder)
        {
            stringBuilder.Append(doubleExpression.Value);
        }
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
            var expressionPrinter = new ExpressionPrinter();
            var stringBuilder = new StringBuilder();
            expressionPrinter.Print(additionExpression, stringBuilder);
            Console.WriteLine(stringBuilder);
        }
    }
}
