using System.Runtime.InteropServices;

namespace Killercodes.Console
{
	public static class Terminal
	{
		const int STD_OUTPUT_HANDLE = -11;
		const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll")]
		static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

		[DllImport("kernel32.dll")]
		static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		public const string RESET = "\x1B[0m";
		public const string UNDERLINE = "\x1B[4m";
		public const string BOLD = "\x1B[1m";
		public const string ITALIC = "\x1B[3m";
		public const string BLINK = "\x1B[5m";
		public const string BLINKRAPID = "\x1B[6m";
		public const string DEFAULTFORE = "\x1B[39m";
		public const string DEFAULTBACK = "\x1B[49m";

		static Terminal()
		{
			var handle = GetStdHandle(STD_OUTPUT_HANDLE);
			uint mode;
			GetConsoleMode(handle, out mode);
			mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
			SetConsoleMode(handle, mode);
		}


		static byte ToColor(int input)
		{
			return (byte)Math.Min((long)input, 255);
		}

		static byte ToColor(long input)
		{
			return (byte)Math.Min((long)input, 255);
		}

		public static void Print(this string text, string prefix = null)
		{
			Console.WriteLine($"{prefix}{text}{RESET}");
		}

		public static string ToText(this string text, bool resetTextToDefault = false)
		{
			if(resetTextToDefault)
				return ($"{text}{RESET}");
			else
				return ($"{text}");
		}

		public static string DefaultColor(this string text)
		{
			return $"{DEFAULTFORE}{DEFAULTBACK}m{text}";
		}

		public static string Color(this string text, (int R, int G, int B) fontColor)
		{
			var red = ToColor(fontColor.R);
			var green = ToColor(fontColor.G);
			var blue = ToColor(fontColor.B);

			return $"\x1B[38;2;{red};{green};{blue}m{text}";
		}

		public static string BackColor(this string text, (int R, int G, int B) backgroundColor)
		{
			var red = ToColor(backgroundColor.R);
			var green = ToColor(backgroundColor.G);
			var blue = ToColor(backgroundColor.B);

			return $"\x1B[48;2;{red};{green};{blue}m{text}";
		}

		public static string Reset(this string text)
		{
			return $"{text}{RESET}";
		}

		public static string Bold(this string text)
		{
			return $"{BOLD}{text}";
		}

		public static string Italic(this string text)
		{
			return $"{ITALIC}{text}";
		}

		public static string Underline(this string text)
		{
			return $"{UNDERLINE}{text}";
		}



		public static string Menu(List<string> options)
		{
			var highlight = (255, 200, 0);
			var bgColor = (55, 55, 55);

			int selectedIndex = 0;
			ConsoleKey key;
			do
			{
				Console.Clear();
				Console.WriteLine($"{BLINK}{BOLD}{BackColor("",bgColor)}{Color("", highlight)}Select an option: (use up/down key) {RESET}");
				for (int i = 0; i < options.Count; i++)
				{
					if (i == selectedIndex)
					{
						string item = $" {i}.{options[i]}   ".BackColor(bgColor).Color(highlight).Underline().ToText(true);
						Console.WriteLine(item);
					}
					else
					{
						string item = $" {i}.{options[i]}".BackColor(bgColor).ToText(true);
						Console.WriteLine(item);
					}
				}

				key = Console.ReadKey(true).Key;

				switch (key)
				{
					case ConsoleKey.UpArrow:
						if (selectedIndex > 0)
						{
							selectedIndex--;
						}
						break;
					case ConsoleKey.DownArrow:
						if (selectedIndex < options.Count - 1)
						{
							selectedIndex++;
						}
						break;
				}
			} while (key != ConsoleKey.Enter);

			Console.Clear();
			Console.WriteLine($"{Terminal.Color("", highlight)} You selected: {options[selectedIndex]}{RESET}");

			return options[selectedIndex];
		}
	}
}
