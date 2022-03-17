using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediator
{
    public class Person
    {
        public string Name;
        public ChatRoom Room;
        private List<string> _chatLog = new();

        public Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string msg = $"{sender}: '{message}'";
            _chatLog.Add(msg);
            Console.WriteLine($"[{Name}'s chat session] {msg}");
        }
    }

    public class ChatRoom
    {
        private List<Person> _people = new();

        public void Join(Person person)
        {
            string joinMsg = $"{person.Name} joined the chat";
            Broadcast("room", joinMsg);

            person.Room = this;
            _people.Add(person);
        }

        public void Broadcast(string roomName, string message)
        {
            foreach (var person in _people)
            {
                if (person.Name != roomName)
                {
                    person.Receive(roomName, message);
                }
            }
        }

        public void Message(string source, string destination, string message)
        {
            _people.FirstOrDefault(p => p.Name == destination)
                ?.Receive(source, message);
        }
    }

    class Program
    {
        static void Main()
        {
            var room = new ChatRoom();

            var john = new Person("John");
            var jane = new Person("Jane");

            room.Join(john);
            room.Join(jane);

            john.Say("Hi");
            jane.Say("Hey there John");

            var simon = new Person("Simon");
            room.Join(simon);
            simon.Say("Hi all, sorry for being late");

            jane.PrivateMessage("Simon","Glad to have you here!");
        }
    }
}
