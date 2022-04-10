using System;

namespace Tx.Core.Extensions.DateTime
{
    public static class DateTimeEx
    {
        public static string ToSystemDatePattern(System.DateTime date) => date.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo?.ShortDatePattern);

        public static bool DifTime(this System.DateTime Date1, System.DateTime Date2, int tolerance)
        {
            System.TimeSpan diffResult = Date1.Subtract(Date2);
            return diffResult.Minutes <= tolerance;
        }

        public static bool IsBetween(this System.DateTime dt, System.DateTime start, System.DateTime end) => dt >= start && dt <= end;

        public static System.DateTime ToDateTime(this double unixTime)
        {
            System.DateTime unixStart = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new System.DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }

        public static double ToUnixTimestamp(this System.DateTime dateTime)
        {
            System.DateTime unixStart = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

    }

}
