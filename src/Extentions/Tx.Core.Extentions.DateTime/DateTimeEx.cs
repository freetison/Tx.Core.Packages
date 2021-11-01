namespace Tx.Core.Extentions.DateTime
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
    }

}
