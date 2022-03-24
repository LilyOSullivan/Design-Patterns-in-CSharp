using System;
using System.Collections.Generic;

namespace HandmadeStateMachine
{
    // Phone call state machine

    public enum State
    {
        OffHook,
        Connecting,
        Connected,
        OnHold
    }

    public enum Trigger
    {
        CallDialed,
        HungUp,
        CallConnected,
        PlacedOnHold,
        TakenOffHold,
        LeftMessage
    }

    class Program
    {
        private static Dictionary<State, List<(Trigger, State)>> rules
            = new Dictionary<State, List<(Trigger, State)>>
            {
                [State.OffHook] = new List<(Trigger, State)> {
                        (Trigger.CallDialed, State.Connecting)
                },
                [State.Connecting] = new List<(Trigger, State)> {
                      (Trigger.HungUp, State.OffHook),
                      (Trigger.CallConnected, State.Connected)
                },
                [State.Connected] = new List<(Trigger, State)> {
                      (Trigger.LeftMessage, State.OffHook),
                      (Trigger.HungUp, State.OffHook),
                      (Trigger.PlacedOnHold, State.OnHold)
                },
                [State.OnHold] = new List<(Trigger, State)>{
                      (Trigger.TakenOffHold, State.Connected),
                      (Trigger.HungUp, State.OffHook)
                }
            };

        static void Main()
        {
            var currentState = State.OffHook;
            while (true)
            {
                Console.WriteLine($"The phone is currently {currentState}");
                Console.WriteLine($"Select a trigger: ");

                for (int i = 0; i < rules[currentState].Count; i++)
                {
                    var (trigger, _) = rules[currentState][i];
                    Console.WriteLine($"{i}. {trigger}");
                }

                int input = int.Parse(Console.ReadLine());

                var (_, state) = rules[currentState][input];
                currentState = state;
                Console.WriteLine();
            }
        }
    }
}
