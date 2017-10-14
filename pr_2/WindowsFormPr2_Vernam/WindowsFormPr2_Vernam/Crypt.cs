using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WindowsFormPr2_Vernam
{
    class Crypt
    {
        public struct TwoBytesString
        {
            public byte[] Text;
            public byte[] Key;
        }

        public TwoBytesString KeyAndText(String input)
        {
            TwoBytesString Output = new TwoBytesString();
            Random fifty = new Random();
            byte[] symbols = Encoding.GetEncoding(1251).GetBytes(input);
            for (int i = 0; i < symbols.Length; i++)
            {
                Debug.Write("Symb n= " + i + " " + symbols[i] + "\n");
            }
            Output.Key = new byte[symbols.Length];
            fifty.NextBytes(Output.Key);
            Output.Text = new byte[symbols.Length];

            for (int i = 0; i < Output.Key.Length; i++)
            {
                Output.Text[i] = Mod(Output.Key[i], symbols[i]);
            }
            return Output;
        }

        public bool[] ByteToBit(byte Byte)
        {
            bool[] Bit = new bool[8];
            byte Counter = 128;
            for (int i = 0; i < Bit.Length; i++)
            {
                if (Byte >= Counter)
                {
                    Bit[i] = true;
                    Byte -= Counter;
                }
                else Bit[i] = false;
                Counter /= 2;
            }
            return Bit;
        }

        public byte BitToByte(bool[] Bit)
        {
            byte Byte = 0;
            byte Counter = 128;
            for (int i = 0; i < Bit.Length; i++)
            {
                if (Bit[i]) Byte += Counter;
                Counter /= 2;
            }
            return Byte;
        }

        public byte Mod (byte a, byte b)
        {
            bool[] BitA = ByteToBit(a);
            bool[] BitB = ByteToBit(b);
            bool[] BitOut = new bool[BitA.Length];
            for (int i = 0; i < BitA.Length; i++)
            {
                BitOut[i] = BitA[i] != BitB[i];
            }
            return BitToByte(BitOut);
        }
    }
}
