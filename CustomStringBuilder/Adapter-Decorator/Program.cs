using System;
using System.Runtime.Serialization;
using System.Text;

namespace Adapter_Decorator
{
    public class CustomStringBuilder
    {
        private StringBuilder _stringBuilder = new StringBuilder();


        public static implicit operator CustomStringBuilder(string s)
        {
            var customStringBuilder = new CustomStringBuilder();
            customStringBuilder._stringBuilder.Append(s);
            return customStringBuilder;
        }

        public static CustomStringBuilder operator +(CustomStringBuilder customStringBuilder, string s)
        {
            customStringBuilder.Append(s);
            return customStringBuilder;
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)_stringBuilder).GetObjectData(info, context);
        }

        public int EnsureCapacity(int capacity)
        {
            return _stringBuilder.EnsureCapacity(capacity);
        }

        public string ToString(int startIndex, int length)
        {
            return _stringBuilder.ToString(startIndex, length);
        }

        public StringBuilder Clear()
        {
            return _stringBuilder.Clear();
        }

        public StringBuilder Append(char value, int repeatCount)
        {
            return _stringBuilder.Append(value, repeatCount);
        }

        public StringBuilder Append(char[] value, int startIndex, int charCount)
        {
            return _stringBuilder.Append(value, startIndex, charCount);
        }

        public StringBuilder Append(string value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(string value, int startIndex, int count)
        {
            return _stringBuilder.Append(value, startIndex, count);
        }

        public StringBuilder AppendLine()
        {
            return _stringBuilder.AppendLine();
        }

        public StringBuilder AppendLine(string value)
        {
            return _stringBuilder.AppendLine(value);
        }

        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            _stringBuilder.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public StringBuilder Insert(int index, string value, int count)
        {
            return _stringBuilder.Insert(index, value, count);
        }

        public StringBuilder Remove(int startIndex, int length)
        {
            return _stringBuilder.Remove(startIndex, length);
        }

        public StringBuilder Append(bool value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(sbyte value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(byte value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(char value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(short value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(int value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(long value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(float value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(double value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(decimal value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(ushort value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(uint value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(ulong value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(object value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Append(char[] value)
        {
            return _stringBuilder.Append(value);
        }

        public StringBuilder Insert(int index, string value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, bool value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, sbyte value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, byte value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, short value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, char value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, char[] value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            return _stringBuilder.Insert(index, value, startIndex, charCount);
        }

        public StringBuilder Insert(int index, int value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, long value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, float value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, double value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, decimal value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, ushort value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, uint value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, ulong value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder Insert(int index, object value)
        {
            return _stringBuilder.Insert(index, value);
        }

        public StringBuilder AppendFormat(string format, object arg0)
        {
            return _stringBuilder.AppendFormat(format, arg0);
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1)
        {
            return _stringBuilder.AppendFormat(format, arg0, arg1);
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            return _stringBuilder.AppendFormat(format, arg0, arg1, arg2);
        }

        public StringBuilder AppendFormat(string format, params object[] args)
        {
            return _stringBuilder.AppendFormat(format, args);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            return _stringBuilder.AppendFormat(provider, format, arg0);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0,
          object arg1)
        {
            return _stringBuilder.AppendFormat(provider, format, arg0, arg1);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0,
          object arg1, object arg2)
        {
            return _stringBuilder.AppendFormat(provider, format, arg0, arg1, arg2);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            return _stringBuilder.AppendFormat(provider, format, args);
        }

        public StringBuilder Replace(string oldValue, string newValue)
        {
            return _stringBuilder.Replace(oldValue, newValue);
        }

        public bool Equals(StringBuilder sb)
        {
            return this._stringBuilder.Equals(sb);
        }

        public StringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            return _stringBuilder.Replace(oldValue, newValue, startIndex, count);
        }

        public StringBuilder Replace(char oldChar, char newChar)
        {
            return _stringBuilder.Replace(oldChar, newChar);
        }

        public StringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            return _stringBuilder.Replace(oldChar, newChar, startIndex, count);
        }

        public int Capacity
        {
            get => _stringBuilder.Capacity;
            set => _stringBuilder.Capacity = value;
        }

        public int MaxCapacity => _stringBuilder.MaxCapacity;

        public int Length
        {
            get => _stringBuilder.Length;
            set => _stringBuilder.Length = value;
        }

        public char this[int index]
        {
            get => _stringBuilder[index];
            set => _stringBuilder[index] = value;
        }
    }

    class Program
    {
        static void Main()
        {
            CustomStringBuilder message = "Hello ";
            message += "world";
            Console.WriteLine(message);
        }
    }
}
