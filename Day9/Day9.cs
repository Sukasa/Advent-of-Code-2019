using IntCodeMachine;

namespace Day9
{
    class Day9
    {
        static void Main(string[] args)
        {
            ICMachine VM = new ICMachine("input.txt");
            //VM.Trace = true;
            VM.Execute();
        }
    }
}
