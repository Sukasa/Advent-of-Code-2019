using System;
using System.Collections.Generic;

namespace IntCodeMachine
{
    public class ICMachine
    {
        int[] State;
        int PC;
        bool Abort;
        Dictionary<int, Action> Opcodes;

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
                Opcodes[instructionRead()]();

        }
    }
}
