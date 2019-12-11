using System;
using System.Collections.Generic;
using System.Drawing;
using IntCodeMachine;

namespace Day11
{
    class Day11
    {
        static void Main(string[] args)
        {
            Dictionary<Point, bool> Painted = new Dictionary<Point, bool>();
            Point Location = new Point(0, 0);
            Point Vector = new Point(0, -1);
            ICMachine Brain = new ICMachine("input.txt");
            Painted[Location] = true;

            Func<bool> IsPainted = () => Painted.ContainsKey(Location) ? Painted[Location] : false;

            Brain.InteractiveMode = false;
            Brain.ExecuteThreaded();
            while (!Brain.Running)
                System.Threading.Thread.Sleep(10);

            while (Brain.Running)
            {
                Brain.ProvideInput(IsPainted() ? 1 : 0);
                Brain.AwaitOutput(2);
                if (!Brain.Running)
                    break;
                Painted[Location] = Brain.GetOutput() == 1 ? true : false;
                switch(Brain.GetOutput())
                {
                    case 0:
                        Vector = RotatePoint(Vector, new Point(0, 0), -90);
                        break;
                    case 1:
                        Vector = RotatePoint(Vector, new Point(0, 0), 90);
                        break;
                }
                Location.X += Vector.X;
                Location.Y += Vector.Y;
            }

            Point BoundsLow = new Point(0, 0);
            foreach(var Extent in Painted.Keys)
            {
                BoundsLow.X = Math.Min(BoundsLow.X, Extent.X);
                BoundsLow.Y = Math.Min(BoundsLow.Y, Extent.Y);
            }

            foreach (var Extent in Painted.Keys)
            {
                Console.SetCursorPosition(Extent.X - BoundsLow.X, Extent.Y - BoundsLow.Y);
                Console.Write(Painted[Extent] ? "#" : ".");
            }
            Console.SetCursorPosition(20, 20);
        }



        static Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
    }
}
