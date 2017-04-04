using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nExtensions
{
	public static class EncryptionExtended
	{
		public static string EncryptToHex (this string input)
		{
			string hex = null;
			char[] values = input.ToCharArray();
			foreach (char c in values)
			{
				int value = Convert.ToInt32(c);
				hex += String.Format("{0:X}", value) + " ";
			}
			return hex;
		}

		public static string DecryptFromHex (this string hex)
		{
			string dhex = null;
			string[] hexValuesSplit = hex.Split(' ');
			foreach (String hex3 in hexValuesSplit)
			{
				int value = Convert.ToInt32(hex3, 16);
				string stringValue = Char.ConvertFromUtf32(value);
				char charValue = (char)value;
				dhex += stringValue;
			}

			return dhex;
		}
	}
}
