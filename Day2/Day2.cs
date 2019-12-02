using System;
using IntCodeMachine;

namespace Day2
{
    class Day2
    {
        static void Main(string[] args)
        {
            ICMachine Machine = new ICMachine();
            int[] InitialState = ICMachine.ParseFile();

            // Apply patches
            InitialState[1] = 12;
            InitialState[2] = 2;

            Machine.LoadState(InitialState);
            Machine.Execute(0);

            Console.WriteLine(Machine.readAddress(0));


            // Part 2, noun/verb search
            for (int noun = 0; noun < 100; noun++)
                for (int verb = 0; verb < 100; verb++)
                {
                    Machine.LoadState(InitialState);

                    Machine.SetNoun(noun);
                    Machine.SetVerb(verb);
                    Machine.Execute();

                    if (Machine.readAddress(0) == 19690720)
                    {
                        Console.WriteLine(100 * noun + verb);
                        return;
                    }
                }
        }
    }
}
