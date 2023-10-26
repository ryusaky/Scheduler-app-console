
using ConsoleAppScheduler.Base.Common;
using Quartz;

namespace ConsoleAppScheduler.Base.Tools
{
    public static class CronExpressionHelper
    {
        public static string AtHourAndMinuteOnGivenDaysOfWeek(int hour, int minute, params DayOfWeek[] daysOfWeek)
        {
            if (daysOfWeek == null || daysOfWeek.Length == 0)
            {
                throw new ArgumentException("You must specify at least one day of week.");
            }

            DateBuilder.ValidateHour(hour);
            DateBuilder.ValidateMinute(minute);

            string cronExpression = $"0 {minute} {hour} ? * ";
            cronExpression+= string.Join(",", daysOfWeek.Select(e => (int)e + 1));
            return cronExpression;
        }
        public static DayOfWeek[] StringDaysToListEnum(string weekDays)
        {
            if (!string.IsNullOrEmpty(weekDays))
            {
                int day = 0;
                List<DayOfWeek> daysOfWeek = new();
                weekDays.ToList().ForEach(r =>
                {
                    if(r.ToString().Equals(Constants.BooleanValues.STRING_TRUE))
                        daysOfWeek.Add(NumberDayToEnum(day));
                    day++;
                });
                return daysOfWeek.ToArray();
            }
            return Array.Empty<DayOfWeek>();
        }

        public static DayOfWeek NumberDayToEnum(int day)
        {
            return day switch
            {
                0 => DayOfWeek.Sunday,
                1 => DayOfWeek.Monday,
                2 => DayOfWeek.Tuesday,
                3 => DayOfWeek.Wednesday,
                4 => DayOfWeek.Thursday,
                5 => DayOfWeek.Friday,
                _ => DayOfWeek.Saturday,
            };
        }
    }
}
