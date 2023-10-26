using System.Globalization;
using static ConsoleAppScheduler.Base.Common.Constants;

namespace ConsoleAppScheduler.Base.Tools
{
    public class DateHelper
    {
        public static DateTime StringToDateFormat(string date, string format = "")
        {
            if (string.IsNullOrEmpty(format))
                format = "dd/MM/yyyy";
            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }
        public static string ToLatinFormat(DateTime date)
        {
            return date.ToString(FormatDates.LATIN_DATE, CultureInfo.InvariantCulture);
        }
    }
}
