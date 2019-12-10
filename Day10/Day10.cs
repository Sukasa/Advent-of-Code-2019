using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day10
{
    class Day10
    {
        static void Main(string[] args)
        {
            bool[][] AsteroidMap = System.IO.File.ReadAllLines("input.txt").Select(x => x.ToCharArray().Select(y => y == '#').ToArray()).ToArray();
            List<Point> Asteroids = new List<Point>();

            for (int y = 0; y < AsteroidMap.Length; y++)
                for (int x = 0; x < AsteroidMap[y].Length; x++)
                {
                    if (AsteroidMap[y][x])
                        Asteroids.Add(new Point(x, y));
                }

            Point Best = new Point(0, 0);
            var Highest = 0;
            var BestIdx = 0;

            for (int a = 0; a < Asteroids.Count; a++)
            {
                var Angles = new List<float>();
                var Count = 0;
                var Source = Asteroids[a];
                for (int b = 0; b < Asteroids.Count; b++)
                {
                    if (a == b)
                    {
                        continue;
                    }

                    var Angle = (float)Math.Atan2(Asteroids[a].X - Asteroids[b].X, Asteroids[a].Y - Asteroids[b].Y);
                    if (!Angles.Contains(Angle))
                    {
                        Angles.Add(Angle);
                        Count++;
                    }
                }

                if (Count > Highest)
                {
                    Best = Asteroids[a];
                    Highest = Count;
                    BestIdx = a;
                }
            }

            Console.WriteLine("{0},{1} = {2}", Best.X, Best.Y, Highest);



            // Part 2
            Console.ReadLine();


            Console.Clear();
            Console.Write(System.IO.File.ReadAllText("input.txt"));

            var Targets = new Dictionary<float, List<Point>>();


            for (int c = 0; c < Asteroids.Count; c++)
            {
                if (c == BestIdx)
                {
                    continue;
                }

                var Angle = (float)Math.Atan2(Asteroids[BestIdx].X - Asteroids[c].X, Asteroids[BestIdx].Y - Asteroids[c].Y);
                if (!Targets.ContainsKey(Angle))
                {
                    Targets[Angle] = new List<Point>();
                }
                Targets[Angle].Add(Asteroids[c]);
            }

            var KeyOrder = Targets.Keys.OrderBy(x => (180 - x) % 180).ToArray();
            var Countdown = 200;
            var KeyIdx = 0;

            Point Last = new Point(-1, -1);



            foreach(var Range in Targets.Values)
            {
                Range.Sort((x, y) => Distance(Best, x).CompareTo(Distance(Best, y)));
            }

            for (; Countdown > 0; Countdown--)
            {
                List<Point> FiringLine;
                do
                {
                    FiringLine = Targets[KeyOrder[KeyIdx]];
                    KeyIdx = (++KeyIdx) % KeyOrder.Length;

                } while (FiringLine.Count == 0);

                Last = FiringLine[0];
                FiringLine.RemoveAt(0);
                Console.SetCursorPosition(Last.X, Last.Y);
                Console.Write(".");

                System.Threading.Thread.Sleep(50);
            }

            Console.Write((Last.X * 100) + Last.Y);

        }

        static float Distance(Point a, Point b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}


