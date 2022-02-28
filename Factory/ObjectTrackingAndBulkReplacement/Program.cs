using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectTrackingAndBulkReplacement
{
    public interface ITheme
    {
        string TextColor { get; }
        string BgrColor { get; }
    }

    class LightTheme : ITheme
    {
        public string TextColor => "black";
        public string BgrColor => "white";
    }

    class DarkTheme : ITheme
    {
        public string TextColor => "white";
        public string BgrColor => "dark gray";
    }

    public class TrackingThemeFactory
    {
        private readonly List<WeakReference<ITheme>> themes = new();

        public ITheme CreateTheme(bool dark)
        {
            ITheme theme = dark ? new DarkTheme() : new LightTheme();

            themes.Add(new WeakReference<ITheme>(theme));
            return theme;
        }

        public string Info
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var reference in themes)
                {
                    if(reference.TryGetTarget(out var theme))
                    {
                        bool dark = theme is DarkTheme;
                        stringBuilder.Append(dark ? "Dark" : "Light").AppendLine(" theme");
                    }
                }
                return stringBuilder.ToString();
            }
        }
    }

    public class ReplaceableThemeFactory
    {
        private readonly List<WeakReference<Ref<ITheme>>> themes = new();

        private ITheme createThemeImpl(bool dark)
        {
            return dark ? new DarkTheme() : new LightTheme();
        }

        public Ref<ITheme> CreateTheme(bool dark)
        {
            var reference = new Ref<ITheme>(createThemeImpl(dark));
            themes.Add(new(reference));
            return reference;
        }

        public void ReplaceTheme(bool dark)
        {
            foreach (var weakReference in themes)
            {
                if(weakReference.TryGetTarget(out var reference))
                {
                    reference.Value = createThemeImpl(dark);
                }
            }
        }
    }

    public class Ref<T> where T:class
    {
        public T Value;

        public Ref(T value)
        {
            Value = value;
        }
    }

    class Program
    {
        static void Main()
        {
            var factory = new TrackingThemeFactory();
            var light = factory.CreateTheme(false);
            var dark = factory.CreateTheme(true);
            Console.WriteLine(factory.Info);

            var factory2 = new ReplaceableThemeFactory();
            var darkReplaceable = factory2.CreateTheme(true);
            Console.WriteLine(darkReplaceable.Value.BgrColor);
            factory2.ReplaceTheme(false);
            Console.WriteLine(darkReplaceable.Value.BgrColor);


        }
    }
}
