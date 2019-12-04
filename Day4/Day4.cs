﻿using System;
using System.Diagnostics;

namespace Day4
{
    class Day4
    {
        static void Main(string[] args)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            for (int test = 0; test < 100; test++)
            {
                int matches = 0;
                int add;
                for (int check = 206938; check <= 679128; check++)
                {
                    add = 0;
                    string Adj = " " + check.ToString() + " ";
                    for (int i = 1; i < 6; i++)
                    {
                        if (Adj[i] > Adj[i + 1])
                            goto Skip;

                        if (add != 0 || ((Adj[i] == Adj[i + 1]) && (Adj[i] != Adj[i + 2]) && (Adj[i] != Adj[i - 1])))
                            add = 1;
                    }

                    matches += add;

                Skip:;
                }

                Console.WriteLine(matches);
            }
            Sw.Stop();
            Console.WriteLine("Avg {0}ms to check {1} possible combinations", (double)Sw.ElapsedMilliseconds / 100, 679128 - 206938 + 1);
        }
    }
}