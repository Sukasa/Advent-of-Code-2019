using System;

namespace Day4
{
    class Day4
    {
        static void Main(string[] args)
        {
            int matches = 0;
            for (int check = 206938; check <= 679128; check++)
            {
                string Adj = " " + check.ToString() + " ";
                if (NoDescending(Adj) && HasDouble(Adj))
                    matches++;
            }

            Console.WriteLine(matches);
        }

        static bool NoDescending(string Input)
        {
            for (int i = 1; i < Input.Length - 2; i++)
                if (Input[i] > Input[i + 1])
                    return false;
            return true;
        }

        static bool HasDouble(string Input)
        {
            int i;
            for (i = 1; i < Input.Length - 2; i++)
                if ((Input[i] == Input[i + 1]) && (Input[i] != Input[i + 2]) && (Input[i] != Input[i - 1]))
                    return true;
            return false;
        }
    }
}
