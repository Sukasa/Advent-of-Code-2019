using System;
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
                int[] checkarr = new int[8];
                checkarr[0] = -1;
                checkarr[7] = -1;
                for (int check = 206938; check <= 679128; check++)
                {
                    add = 0;

                    checkarr[6] = check % 10;
                    checkarr[5] = (check / 10) % 10;
                    checkarr[4] = (check / 100) % 10;
                    checkarr[3] = (check / 1000) % 10;
                    checkarr[2] = (check / 10000) % 10;
                    checkarr[1] = (check / 100000);

                    for (int i = 1; i < 6; i++)
                    {
                        if (checkarr[i] > checkarr[i + 1])
                            goto Skip;

                        if (add != 0 || ((checkarr[i] == checkarr[i + 1]) && (checkarr[i] != checkarr[i + 2]) && (checkarr[i] != checkarr[i - 1])))
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
