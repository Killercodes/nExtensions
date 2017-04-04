using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nExtensions
{
    public static class DateExtention
    {
		/// <summary>
		/// Convert to EPOCH
		/// </summary>
		/// <returns>long EPOCH</returns>
        public static long ToEpoch (this DateTime value)
        {
            long epoch = (long)(value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            return epoch;
        }

		/// <summary>
		/// Convert from EPOCH to Datetime
		/// </summary>
		/// <returns>object DateTime</returns>
        private static DateTime EpochToString (this long epoch)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch);
            return dateTime;
        }

		private static string ToGMT (this DateTime dateTime)
		{
			return dateTime.ToString("R");
		}
    }
}
