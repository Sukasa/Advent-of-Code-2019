using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace IntCodeMachine
{
    public class ICMachine
    {

        #region Variables

        int[] State;
        int PC;
        bool Abort;
        Dictionary<int, Action> Opcodes;
        Action[] Operands;
        int OpParams;

        #endregion

        #region Multithreading Support

        AutoResetEvent InputEvent = new AutoResetEvent(false);
        AutoResetEvent OutputEvent = new AutoResetEvent(false);
        AutoResetEvent AbortEvent = new AutoResetEvent(false);

        #endregion

        #region Properties

        public Queue<int> Output { get; } = new Queue<int>();
        public Queue<int> Input { get; } = new Queue<int>();

        public bool InteractiveMode { get; set; } = true;
        public bool Running { get; private set; } = false;

        #endregion

        #region Public I/O Handling

        public void AwaitOutput()
        {
            OutputEvent.WaitOne();
        }

        public void ProvideInput(int Value)
        {
            Input.Enqueue(Value);
            InputEvent.Set();
        }

        #endregion

        #region Configuration & Operation

        public static int[] ParseFile(string Filename = "input.txt") => System.IO.File.ReadAllText(Filename).Split(',').Select(x => int.Parse(x)).ToArray();

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
            Halt();
            while (Running)
                Thread.Sleep(10);

            State = new int[NewState.Length];
            Array.Copy(NewState, 0, State, 0, NewState.Length);
            Input.Clear();
            Output.Clear();
            AbortEvent.Reset();
            InputEvent.Reset();
            OutputEvent.Reset();
        }

        public void ExecuteThreaded(int ProgramCounter = 0, string ThreadName = "VM Thread")
        {
            Thread Async = new Thread((x) => Execute((int)x));
            Async.Name = ThreadName;
            Async.Start(ProgramCounter);
        }

        public void Execute(int ProgramCounter = 0)
        {
            PC = ProgramCounter;
            Abort = false;
            Running = true;

            while (!Abort)
            {
                int Instruction = (OpParams = instructionRead()) % 100;
                OpParams /= 100;
                Operands[Instruction]();
            }

            Running = false;
        }

        public void Resume() => Execute(PC);

        public void Halt()
        {
            Abort = true;
            AbortEvent.Set();
        }

        #endregion

        #region Construction & Operands

        public ICMachine(string Filename) : this()
        {
            LoadState(ParseFile(Filename));
        }

        public ICMachine()
        {
            Opcodes = new Dictionary<int, Action> {
                // 1 - Add
                {1,  () => {int Addr1 = instructionRead(); int Addr2 = instructionRead(); writeParameter(instructionRead(), readParameter(Addr1) + readParameter(Addr2));}},

                //2 - Multiply
                {2,  () => {int Addr1 = instructionRead(); int Addr2 = instructionRead(); writeParameter(instructionRead(), readParameter(Addr1) * readParameter(Addr2));}},

                // 3 - Write input to memory
                {3, () => { GetInput(); } },

                // 4 - Diagnostic output
                {4, () => { Output.Enqueue(readParameter()); OutputEvent.Set(); } },

                // 5 - Jump if true
                {5, () => { if (readParameter() != 0) PC = readParameter(); else PC++; } },

                // 6 - Jump if false
                {6, () => { if (readParameter() == 0) PC = readParameter(); else PC++; } },
                
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

        #endregion

        #region Internal Support Functions

        private void GetInput()
        {
            if (Input.Count == 0 && InteractiveMode)
            {
                Console.Write("Input Requested: ");
                writeAddress(instructionRead(), int.Parse(Console.ReadLine()));
            }
            else
            {
                int Which = 0;
                if (Input.Count == 0)
                    Which = WaitHandle.WaitAny(new WaitHandle[] { InputEvent, AbortEvent });
                else
                    InputEvent.Reset();

                if (Which == 1)
                    return;

                writeAddress(instructionRead(), Input.Dequeue());
            }
        }

        internal int instructionRead()
        {
            return State[PC++];
        }

        private int readAddress(int Address)
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

        private void writeAddress(int Address, int Value)
        {
            if (Address < 0)
                Address = 0;
            if (Address >= State.Length)
                Array.Resize(ref State, Address + 1);

            State[Address] = Value;
        }

        #endregion

    }
}
