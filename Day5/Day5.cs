using IntCodeMachine;
using System;

namespace Day5
{
    class Day5
    {
        static void Main(string[] args)
        {
            ICMachine Machine = new ICMachine();
            Machine.LoadState(ICMachine.ParseFile());
            Machine.Execute();
        }
    }
}
