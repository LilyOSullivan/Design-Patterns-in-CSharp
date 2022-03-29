using System;
using System.Text;

namespace IntrusiveVisitor
{
    public abstract class Expression
    {
        public abstract void Print(StringBuilder stringBuilder);
    }

    public class DoubleExpression : Expression
    {
        private double value;
        public DoubleExpression(double value)
        {
            this.value = value;
        }

        public override void Print(StringBuilder stringBuilder)
        {
            stringBuilder.Append(value);
        }
    }

    public class AdditionExpression : Expression
    {
        private Expression left, right;

        public AdditionExpression(Expression left, Expression right)
        {
            this.left = left ?? throw new ArgumentNullException(nameof(left));
            this.right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override void Print(StringBuilder stringBuilder)
        {
            stringBuilder.Append('(');
            left.Print(stringBuilder);
            stringBuilder.Append('+');
            right.Print(stringBuilder);
            stringBuilder.Append(')');
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
            var stringBuilder = new StringBuilder();
            additionExpression.Print(stringBuilder);
            Console.WriteLine(stringBuilder);
        }
    }
}
