using System;
using System.Text;

namespace AcyclicVisitor
{
    public interface IVisitor<TVisitable>
    {
        void Visit(TVisitable obj);
    }

    public interface IVisitor { }

    public abstract class Expression
    {
        public virtual void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Expression> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class DoubleExpression : Expression
    {
        public double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<DoubleExpression> typed)
                typed.Visit(this);
        }
    }

    public class AdditionExpression : Expression
    {
        public Expression Left, Right;

        public AdditionExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<AdditionExpression> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class ExpressionPrinter : IVisitor,
      IVisitor<Expression>,
      IVisitor<DoubleExpression>,
      IVisitor<AdditionExpression>
    {
        StringBuilder stringBuilder = new StringBuilder();

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

        public void Visit(Expression expression)
        {

        }

        public override string ToString() => stringBuilder.ToString();
    }

    public class Demo
    {
        public static void Main()
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
        }
    }
}
