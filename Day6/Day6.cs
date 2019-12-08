using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Day6
    {
        static void Main(string[] args)
        {
            List<string> Orbits = new List<string>(System.IO.File.ReadAllLines("input.txt"));
            Dictionary<string, int> Tree = new Dictionary<string, int>() { { "COM", 0 } };
            Dictionary<string, string> Path = new Dictionary<string, string>();


            // Part 1
            while (Orbits.Count > 0)
            {
                for(int i = Orbits.Count - 1; i >= 0; i--)
                {
                    var Orbit = Orbits[i].Split(")");
                    if (Tree.ContainsKey(Orbit[0]))
                    {
                        Tree.Add(Orbit[1], Tree[Orbit[0]] + 1);
                        Path.Add(Orbit[1], Orbit[0]);
                        Orbits.RemoveAt(i);
                    }
                }
            }

            Console.WriteLine(Tree.Values.Sum());

            // Part 2

            var YouAt = "YOU";
            var OrbitChanges = -1;

            while (true)
            {
                YouAt = Path[YouAt];
                OrbitChanges++;
                var SantaAt = Path["SAN"];
                var SantaChanges = 0;
                while(Path.ContainsKey(SantaAt))
                {
                    SantaAt = Path[SantaAt];
                    SantaChanges++;
                    if (YouAt == SantaAt)
                    {
                        Console.WriteLine(OrbitChanges + SantaChanges);
                        return;
                    }
                }
            }

        }
    }
}
