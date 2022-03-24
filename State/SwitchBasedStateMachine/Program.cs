using System;
using System.Text;

namespace SwitchBasedStateMachine
{
    public enum State
    {
        Locked,
        Failed,
        Unlocked
    }

    class Program
    {
        static void Main()
        {
            string code = "1234";
            var state = State.Locked;
            var stringBuilder = new StringBuilder();

            while (true)
            {
                switch (state)
                {
                    case State.Locked:
                        stringBuilder.Append(Console.ReadKey().KeyChar);

                        if (stringBuilder.ToString() == code)
                        {
                            state = State.Unlocked;
                            break;
                        }

                        if (!code.StartsWith(stringBuilder.ToString()))
                        {
                            state = State.Failed;
                            //goto case State.Failed;
                        }
                        break;
                    case State.Failed:
                        Console.CursorLeft = 0;
                        Console.WriteLine("Failed");
                        stringBuilder.Clear();
                        state = State.Locked;
                        break;
                    case State.Unlocked:
                        Console.CursorLeft = 0;
                        Console.WriteLine("UNLOCKED");
                        return;
                }
            }
        }
    }
}
