using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nExtensions
{
	public static class FileExtended
	{

		public static string ReadFile (this FileStream fileStream)
		{
			var sr = new StreamReader(fileStream);
			sr.BaseStream.Seek(0, SeekOrigin.Begin);
			var str = sr.ReadToEnd();
			while (str == null)
			{
				str = sr.ReadLine();
			}
			sr.Close();
			return str;
		}

		public static void WriteFile (this FileStream fileStream, string content)
		{
			if (fileStream.CanWrite)
			{
				fileStream.Unlock(0, fileStream.Length);
				using (StreamWriter SW = new StreamWriter(fileStream))
				{
					SW.Write(content);
					SW.Flush();
					SW.Close();
				}
			}
		}
	}
}
