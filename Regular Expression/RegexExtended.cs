using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nExtensions
{
	public static class RegexExtended
	{
		public static bool IsMatch (this string stringText,string regularExpressionPattern)
		{
			Regex r = new Regex(regularExpressionPattern);
			return (r.IsMatch(stringText));
		}




	}
}
