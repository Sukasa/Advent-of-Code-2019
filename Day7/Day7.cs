using IntCodeMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Day7
{
    class Day7
    {
        static void Main(string[] args)
        {
            ICMachine[] VMs = new ICMachine[] { new ICMachine(), new ICMachine(), new ICMachine(), new ICMachine(), new ICMachine() };
            var BaseState = ICMachine.ParseFile();

            // Part 1
            var Highest = 0;
            foreach (var Permutation in Permutations(new int[] { 0, 1, 2, 3, 4 }))
            {
                var PrevOutput = 0;
                for (int i = 0; i < 5; i++)
                {

                    VMs[i].LoadState(BaseState);
                    var Input = VMs[i].Input;

                    Input.Enqueue(Permutation[i]);
                    Input.Enqueue(PrevOutput);
                    VMs[i].Execute();
                    PrevOutput = (int)VMs[i].Output.Dequeue();
                }

                if (PrevOutput > Highest)
                {
                    Highest = PrevOutput;
                }

            }

            Console.WriteLine(Highest);

            // Part 2
            Highest = 0;
            foreach (var Permutation in Permutations(new int[] { 5, 6, 7, 8, 9 }))
            {
                Console.Write("Testing permutation: {0} {1} {2} {3} {4} = ", Permutation[0], Permutation[1], Permutation[2], Permutation[3], Permutation[4]);

                for (int i = 0; i < 5; i++)
                {
                    VMs[i].LoadState(BaseState);
                    VMs[i].InteractiveMode = false;
                    VMs[i].Input.Enqueue(Permutation[i]);
                    VMs[i].ExecuteThreaded(ThreadName: "VM " + i.ToString());
                }

                while (VMs.Any(x => !x.Running)) // Allow VM threads to initialize
                    Thread.Sleep(10);

                int Output = 0;
                int Iterations = 0;

                while (VMs.All(x => x.Running))
                {
                    Iterations++;
                    for (int i = 0; i < 5; i++)
                    {
                        VMs[i].ProvideInput(Output);
                        VMs[i].AwaitOutput();
                        Output = (int)VMs[i].Output.Dequeue();
                    }
                }

                Highest = Math.Max(Highest, Output);
                Console.WriteLine("{0} ({1} iterations)", Output, Iterations);
            }

            Console.WriteLine();
            Console.WriteLine(Highest);
        }


        static IEnumerable<int[]> Permutations(IEnumerable<int> Input)
        {
            if (Input.Count() == 1)
                yield return new int[] { Input.First() };
            else
                foreach (var Element in Input)
                {
                    var Further = Permutations(Input.Where(x => x != Element));
                    foreach (var sub in Further)
                    {
                        IEnumerable<int> Base = new List<int>() { Element };
                        yield return Base.Union(sub).ToArray();
                    }

                }
        }
    }
}
