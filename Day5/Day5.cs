using IntCodeMachine;
using System;

namespace Day5
{
    class Day5
    {
        static void Main(string[] args)
        {
            ICMachine Machine = new ICMachine("input.txt");
            Machine.Execute();
        }
    }
}
