using System;

namespace WeakEventPattern
{
    public class Button
    {
        public event EventHandler Clicked;

        public void Click()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        public Window(Button button)
        {
            button.Clicked += ButtonOnClicked;
        }

        private void ButtonOnClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked Button (Window Handler)");
        }

        ~Window()
        {
            Console.WriteLine("Window Finalized");
        }
    }

    class Program
    {
        static void Main()
        {
            var button = new Button();
            var window = new Window(button);
            var windowRef = new WeakReference(window);

            button.Click();

            Console.WriteLine($"Setting window to null");
            window = null;

            CallGC();
            Console.WriteLine($"Is window in memory? {windowRef.IsAlive}");
        }

        private static void CallGC()
        {
            Console.WriteLine("Starting Garbage Collection");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("Finished Garbage Collection");

        }
    }
}
