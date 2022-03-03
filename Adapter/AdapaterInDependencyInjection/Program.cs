using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.Metadata;

namespace AdapaterInDependencyInjection
{
    public interface ICommand
    {
        void Execute();
    }

    class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving a file");
        }
    }

    class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Opening a file");
        }
    }

    public class Button
    {
        private ICommand _command;
        private string _name;

        public Button(ICommand command, string name)
        {
            this._command = command ?? throw new ArgumentNullException(nameof(command));
            this._name = name;
        }

        public void Click()
        {
            _command.Execute();
        }

        public void PrintMe()
        {
            Console.WriteLine($"I am a button called {_name}");
        }
    }

    public class Editor
    {
        private IEnumerable<Button> _buttons;
        public IEnumerable<Button> Buttons => _buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            _buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));
        }

        public void clickAll()
        {
            foreach (Button button in _buttons)
            {
                button.Click();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<SaveCommand>().As<ICommand>()
                .WithMetadata("Name", "Save");
            containerBuilder.RegisterType<OpenCommand>().As<ICommand>()
                .WithMetadata("Name", "Open");
            //containerBuilder.RegisterType<Button>();
            //containerBuilder.RegisterAdapter<ICommand,Button>(cmd => new Button(cmd));
            containerBuilder.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
                 new Button(cmd.Value, (string)cmd.Metadata["Name"])
            );
            containerBuilder.RegisterType<Editor>();

            using (var container = containerBuilder.Build())
            {
                var editor = container.Resolve<Editor>();
                foreach(var button in editor.Buttons)
                {
                    button.PrintMe();
                }
            }
        }
    }
}
