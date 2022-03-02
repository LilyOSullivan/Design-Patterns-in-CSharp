using System;
using System.Collections.Generic;
using System.Text;

namespace AmbientContext
{
    public sealed class BuildingContext : IDisposable
    {
        public int WallHeight;
        private static Stack<BuildingContext> stack = new();

        static BuildingContext()
        {
            stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        public static BuildingContext Current => stack.Peek();

        public void Dispose()
        {
            if (stack.Count > 1)
            {
                stack.Pop();
            }
        }
    }

    public class Building
    {
        public List<Wall> Walls = new List<Wall>();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var wall in Walls)
            {
                stringBuilder.AppendLine(wall.ToString());
            }
            return stringBuilder.ToString();
        }
    }

    public struct Point
    {
        private int _x, _y;
        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"{nameof(_x)}: {_x}, {nameof(_y)}: {_y}";
        }
    }

    public class Wall
    {
        public Point Start, End;
        public int Height;

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight;
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Height)}: {Height}";
        }
    }

    class Program
    {
        static void Main()
        {
            var house = new Building();

            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                using (new BuildingContext(3500))
                {
                    // 1st Floor
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                }
                // Ground Floor
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }
            Console.WriteLine(house);
        }
    }
}
