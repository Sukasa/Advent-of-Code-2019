using IntCodeMachine;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
namespace Day3
{
    class Day3
    {
        static void Main(string[] args)
        {
            string[][] Directions = File.ReadAllLines("input.txt").Select(x => x.Split(',')).ToArray();
            var MinY = 0;
            var MinX = 0;
            var MaxY = 0;
            var MaxX = 0;

            // Follow the wires to find max bounds
            var First = FindBounds(Directions[0]);
            var Second = FindBounds(Directions[1]);

            MinY = Math.Min(First.MinY, Second.MinY);
            MinX = Math.Min(First.MinX, Second.MinX);
            MaxY = Math.Max(First.MaxY, Second.MaxY);
            MaxX = Math.Max(First.MaxX, Second.MaxX);

            int Stride = (MaxX - MinX) + 1;
            int Height = (MaxY - MinY) + 1;
            byte[] Checks = new byte[Height * Stride];
            int[][] SigLatencies = new int[2][];
            SigLatencies[0] = new int[Height * Stride];
            SigLatencies[1] = new int[Height * Stride];

            PlotFirstPath(Stride, MinY, MinX, Checks, Directions[0], SigLatencies[0]);
            PlotSecondPath(Stride, MinY, MinX, Checks, Directions[1], SigLatencies[1]);

            //RenderDebug(Stride, Height, Checks);
            // Intersections found at this point.  Now to find the nearest to 0,0
            FindNearestIntersection(Stride, Height, MinY, MinX, Checks, SigLatencies);
        }

        public static void RenderDebug(int Width, int Height, byte[] Field)
        {
            Bitmap Test = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(Test);
            G.FillRectangle(new SolidBrush(Color.DarkSlateGray), 0, 0, Width, Height);
            var Colors = new Color[] { Color.DarkRed, Color.DarkGreen, Color.White };
            for(int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Field[(y * Width) + x] > 0)
                        Test.SetPixel(x, y, Colors[Field[(y * Width) + x] - 1]);
                }
            }

            Test.Save("debug.png");

        }

        public static void FindNearestIntersection(int Stride, int Height, int MinY, int MinX, byte[] field, int[][]LatencyMaps)
        {
            int Distance = int.MaxValue;
            int CenterX = -MinX;
            int CenterY = -MinY;
            int Latency = int.MaxValue;
            int LatencyX = 0;
            int LatencyY = 0;

            for (int X = 0; X < Stride; X++)
            {
                for (int Y = 0; Y < Height; Y++)
                {
                    if (field[(Y * Stride) + X] == 3)
                    {
                        Console.WriteLine("Intersection at {0},{1}", X - CenterX, Y - CenterY);
                        int NewDistance = Math.Abs(CenterY - Y) + Math.Abs(CenterX - X);
                        if (NewDistance < Distance)
                            Distance = NewDistance;
                        int NewLatency = LatencyMaps[0][(Y * Stride) + X] + LatencyMaps[1][(Y * Stride) + X];
                        if (Latency > NewLatency)
                        {
                            Latency = NewLatency;
                            LatencyX = X - CenterX;
                            LatencyY = Y - CenterY;
                        }
                    }
                }
            }

            Console.WriteLine("Nearest is {0}.  Lowest latency is {1},{2} ({3})", Distance, LatencyX, LatencyY, Latency);
        }


        public static void PlotFirstPath(int Stride, int MinY, int MinX, byte[] Field, string[] Path, int[] LatencyMap)
        {
            int Y = -MinY;
            int X = -MinX;
            int Latency = 0;

            foreach (var Step in Path)
            {
                int Steps = int.Parse(Step.Substring(1));
                switch (Step[0])
                {
                    case 'U':
                        while (Steps > 0)
                        {
                            Y--;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] = 1;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ?  Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                    case 'D':
                        while (Steps > 0)
                        {
                            Y++;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] = 1;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                    case 'L':
                        while (Steps > 0)
                        {
                            X--;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] = 1;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                    case 'R':
                        while (Steps > 0)
                        {
                            X++;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] = 1;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                }
            }

        }


        public static void PlotSecondPath(int Stride, int MinY, int MinX, byte[] Field, string[] Path, int[] LatencyMap)
        {
            int Y = -MinY;
            int X = -MinX;
            int Latency = 0;

            foreach (var Step in Path)
            {
                int Steps = int.Parse(Step.Substring(1));
                switch (Step[0])
                {
                    case 'U':
                        while (Steps > 0)
                        {
                            Y--;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] |= 2;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                    case 'D':
                        while (Steps > 0)
                        {
                            Y++;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] |= 2;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                    case 'L':
                        while (Steps > 0)
                        {
                            X--;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] |= 2;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                    case 'R':
                        while (Steps > 0)
                        {
                            X++;
                            Latency++;
                            Steps--;
                            Field[(Y * Stride) + X] |= 2;
                            LatencyMap[(Y * Stride) + X] = LatencyMap[(Y * Stride) + X] == 0 ? Latency : LatencyMap[(Y * Stride) + X];
                        }
                        break;
                }
            }

        }

        public static (int MinY, int MinX, int MaxY, int MaxX) FindBounds(string[] Path)
        {
            var MinY = 0;
            var MinX = 0;
            var MaxY = 0;
            var MaxX = 0;

            int X = 0;
            int Y = 0;

            foreach (var Step in Path)
            {
                switch (Step[0])
                {
                    case 'U':
                        Y -= int.Parse(Step.Substring(1));
                        break;
                    case 'D':
                        Y += int.Parse(Step.Substring(1));
                        break;
                    case 'L':
                        X -= int.Parse(Step.Substring(1));
                        break;
                    case 'R':
                        X += int.Parse(Step.Substring(1));
                        break;
                }
                MinY = Math.Min(MinY, Y);
                MinX = Math.Min(MinX, X);
                MaxY = Math.Max(MaxY, Y);
                MaxX = Math.Max(MaxX, X);
            }

            return (MinY, MinX, MaxY, MaxX);
        }
    }
}
