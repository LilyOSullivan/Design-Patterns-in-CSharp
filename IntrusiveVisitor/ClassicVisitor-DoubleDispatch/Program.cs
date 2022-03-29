using System;
using System.Collections.Generic;
using System.Text;

namespace ClassicVisitor_DoubleDispatch
{
    public interface IExpressionVisitor
    {
        void Visit(DoubleExpression doubleExpression);
        void Visit(AdditionExpression additionExpression);
    }

    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }

    public class DoubleExpression : Expression
    {
        public double Value;
        public DoubleExpression(double value)
        {
            this.Value = value;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            // Double dispatch
            visitor.Visit(this);
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

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ExpressionPrinter : IExpressionVisitor
    {
        private StringBuilder stringBuilder = new();

        public void Visit(DoubleExpression doubleExpression)
        {
            stringBuilder.Append(doubleExpression.Value);
        }

        public void Visit(AdditionExpression additionExpression)
        {
            stringBuilder.Append('(');
            additionExpression.Left.Accept(this);
            stringBuilder.Append('+');
            additionExpression.Right.Accept(this);
            stringBuilder.Append(')');
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }

    public class ExpressionCalculator : IExpressionVisitor
    {
        public double Result;

        public void Visit(DoubleExpression doubleExpression)
        {
            Result = doubleExpression.Value;
        }

        public void Visit(AdditionExpression additionExpression)
        {
            additionExpression.Left.Accept(this);
            var a = Result;
            additionExpression.Right.Accept(this);
            var b = Result;
            Result = a + b;
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
            expressionPrinter.Visit(additionExpression);
            Console.WriteLine(expressionPrinter);

            var calculator = new ExpressionCalculator();
            calculator.Visit(additionExpression);
            Console.WriteLine($"{expressionPrinter} = {calculator.Result}");
        }
    }
}
