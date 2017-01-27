using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nExtensions
{
    public static class DateExtention
    {
        public static long ToEpoch (this DateTime value)
        {
            long epoch = (long)(value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            return epoch;
        }

        private static DateTime EpochToString (this long epoch)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch);
            return dateTime;
        }
    }
}
