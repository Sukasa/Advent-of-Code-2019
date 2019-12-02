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

        public ICMachine()
        {
            Opcodes = new Dictionary<int, Action> {
                // 1 - Add
                {1,  () => {int Addr1 = instructionRead(); int Addr2 = instructionRead(); writeAddress(instructionRead(), readAddress(Addr1) + readAddress(Addr2));}},

                //2 - Multiply
                {2,  () => {int Addr1 = instructionRead(); int Addr2 = instructionRead(); writeAddress(instructionRead(), readAddress(Addr1) * readAddress(Addr2));}},

                // 99 - Halt
                {99,  () => {Abort = true;}},
            };

            // Convert from sparse dictionary to flat array
            Operands = new Action[Opcodes.Keys.Max() + 1];
            Action InvalidOpcode = () => { Console.WriteLine("Invalid opcode {0} as {1}", readAddress(PC - 1), PC); Abort = true; };

            for(int i = 0; i < Operands.Length; i++)
                Operands[i] = InvalidOpcode;

            foreach(var Opcode in Opcodes)
                Operands[Opcode.Key] = Opcode.Value;
        }

        public ICMachine(int[] StartingState) : this()
        {
            LoadState(StartingState);
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
                Operands[instructionRead()]();

        }
    }
}
