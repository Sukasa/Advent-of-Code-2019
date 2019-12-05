using System;
using System.Collections.Generic;
using System.Linq;

namespace IntCodeMachine
{
    public class ICMachine
    {
        int[] State;
        int PC;
        bool Abort;
        Dictionary<int, Action> Opcodes;
        Action[] Operands;
        int OpParams;

        public static int[] ParseFile(string Filename = "input.txt") => System.IO.File.ReadAllText(Filename).Split(',').Select(x => int.Parse(x)).ToArray();

        public ICMachine()
        {
            Opcodes = new Dictionary<int, Action> {
                // 1 - Add
                {1,  () => {int Addr1 = instructionRead(); int Addr2 = instructionRead(); writeParameter(instructionRead(), readParameter(Addr1) + readParameter(Addr2));}},

                //2 - Multiply
                {2,  () => {int Addr1 = instructionRead(); int Addr2 = instructionRead(); writeParameter(instructionRead(), readParameter(Addr1) * readParameter(Addr2));}},

                // 3 - Write input to memory
                {3, () => { Console.Write("Input Requested: "); writeAddress(instructionRead(), int.Parse(Console.ReadLine())); } },

                // 4 - Diagnostic output
                {4, () => { Console.WriteLine("Diagnostic output: {0}", readParameter()); } },

                // 5 - Jump if true
                {5, () => { if (readParameter() != 0) { PC = readParameter(); } else { PC++; } } },

                // 6 - Jump if false
                {6, () => { if (readParameter() == 0) { PC = readParameter(); } else { PC++; } } },
                
                // 7 - Less Than
                {7, () => { int Param1 = readParameter(); int Param2 = readParameter(); writeAddress(instructionRead(), (Param1 < Param2) ? 1 : 0); } },
                
                // 8 - Equals
                {8, () => { int Param1 = readParameter(); int Param2 = readParameter(); writeAddress(instructionRead(), (Param1 == Param2) ? 1 : 0); } },

                // 99 - Halt
                {99,  () => {Abort = true;}},
            };

            // Convert from sparse dictionary to flat array
            Operands = new Action[Opcodes.Keys.Max() + 1];
            Action InvalidOpcode = () => { PC--; Console.WriteLine("Invalid opcode {0} at {1}", readAddress(PC), PC); Abort = true; };

            for (int i = 0; i < Operands.Length; i++)
                Operands[i] = InvalidOpcode;

            foreach (var Opcode in Opcodes)
                Operands[Opcode.Key] = Opcode.Value;
        }

        internal int instructionRead()
        {
            return State[PC++];
        }

        public int readAddress(int Address)
        {
            if (Address < 0)
                Address = 0;
            if (Address >= State.Length)
                Array.Resize(ref State, Address + 1);

            return State[Address];
        }

        private int readParameter() => readParameter(instructionRead());

        private int readParameter(int Param)
        {
            int ParamMode = OpParams % 10;
            OpParams /= 10;

            switch (ParamMode)
            {
                case 0:
                    return readAddress(Param);
                case 1:
                    return Param;
                default:
                    return Param;
            }
        }
        private void writeParameter(int Param, int Value)
        {
            int ParamMode = OpParams % 10;
            OpParams /= 10;

            switch (ParamMode)
            {
                case 0:
                    writeAddress(Param, Value);
                    break;
            }
        }

        public void writeAddress(int Address, int Value)
        {
            if (Address < 0)
                Address = 0;
            if (Address >= State.Length)
                Array.Resize(ref State, Address + 1);

            State[Address] = Value;
        }

        public void SetVerb(int Verb)
        {
            writeAddress(2, Verb);
        }

        public void SetNoun(int Noun)
        {
            writeAddress(1, Noun);
        }

        public void LoadState(int[] NewState)
        {
            State = new int[NewState.Length];
            Array.Copy(NewState, 0, State, 0, NewState.Length);
        }

        public void Execute(int ProgramCounter = 0)
        {
            PC = ProgramCounter;
            Abort = false;

            while (!Abort)
            {
                int Instruction = (OpParams = instructionRead()) % 100;
                OpParams /= 100;
                Operands[Instruction]();
            }

        }

        public void Resume() => Execute(PC);
    }
}
