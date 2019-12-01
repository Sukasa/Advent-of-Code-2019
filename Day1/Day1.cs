using System.IO;
using System;
using System.Linq;

namespace Day1
{
    class Day1
    {
        static int FuelNeed(int Fuel)
        {
            return Fuel / 3 - 2;
        }

        static void Main(string[] args)
        {
            var FuelRequirements = File.ReadLines("input.txt").Select(x => FuelNeed(int.Parse(x)));
            var SumFuel = FuelRequirements.Sum();
            Console.Write("Naive fuel req: ");
            Console.WriteLine(SumFuel);
            Console.WriteLine("");
            while (FuelRequirements.Any())
            {
                FuelRequirements = FuelRequirements.Select(x => FuelNeed(x)).Where(x => x > 0);
                SumFuel += FuelRequirements.Sum();
            }

            Console.Write("Cascading fuel req: ");
            Console.WriteLine(SumFuel);
            Console.WriteLine("");

            Console.ReadLine();
        }
    }
}
