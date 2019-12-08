using System;
using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    class Day8
    {
        public const int Width = 25;
        public const int Height = 6;
        public const int Size = Width * Height;
        static void Main(string[] args)
        {

            List<Layer> Layers = new List<Layer>();
            byte[] Data = System.IO.File.ReadAllBytes("input.txt").Select(x => (byte)(x - '0')).ToArray();

            Layer Current = null; ;
            for (int i = 0; i < Data.Length - 1; i++)
            {
                if (i % Size == 0)
                    Layers.Add(Current = new Layer());
                Current.ImageData[i % Size] = Data[i];
                Current.Distribution[Data[i]]++;
            }
            Current = Layers.OrderBy(x => x.Distribution[0]).First();
            Console.Write(Current.Distribution[1] * Current.Distribution[2]);


            for (int i = 0; i < Size; i++)
            {
                if (i % Width == 0)
                    Console.WriteLine();
                foreach (var Layer in Layers)
                {
                    if (Layer.ImageData[i] != 2)
                    {
                        Console.Write(Layer.ImageData[i] == 0 ? " " : "*");
                        break;
                    }
                }
            }
            
        }

        class Layer
        {
            public int[] ImageData;
            public int[] Distribution;

            public Layer()
            {
                ImageData = new int[Width * Height];
                Distribution = new int[10];
            }
        }
    }
}
