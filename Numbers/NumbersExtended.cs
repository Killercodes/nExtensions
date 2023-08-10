using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace nExtensions
{
    public static class NumbersExtended
    {
        public static string ToBinary (this long value)
        {
            var result = Convert.ToString(value, 2).PadLeft(8, '0');
            return result;
        }

        public static string ToOct (this int value)
        {
            var result = Convert.ToString(value, 8);
            return result;
        }
        public static string ToHex(this int value)
        {
            var result = Convert.ToString(value, 16);
            return result; 
        }

        public static string BinaryToHex (this string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        

    }
}
