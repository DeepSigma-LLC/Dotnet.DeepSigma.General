
namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for DateTime operations.
/// </summary>
public static class DateTimeExtension
{
    private static string _dateStringFormat { get; } = "yyyy-MM-dd 'T 'HH-mm-ss";

    extension(DateTime)
    {
        /// <summary>
        /// Calculates the number of days between two dates using the 360-day year convention (30-day months).
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="methodEu">True for European method, false for US (NASD) method.</param>
        /// <returns>The number of days between the two dates based on a 360-day year.</returns>
        public static int Days360(DateTime startDate, DateTime endDate, bool methodEu = false)
        {
            int startDay = startDate.Day;
            int startMonth = startDate.Month;
            int startYear = startDate.Year;

            int endDay = endDate.Day;
            int endMonth = endDate.Month;
            int endYear = endDate.Year;

            // Apply rules for the start date
            if (startDay == 31 || (!methodEu && IsLastDayOfFebruary(startDate)))
            {
                startDay = 30;
            }

            // Apply rules for the end date
            if (endDay == 31)
            {
                if (!methodEu && startDay != 30)
                {
                    endDay = 1;
                    if (endMonth == 12)
                    {
                        endYear++;
                        endMonth = 1;
                    }
                    else
                    {
                        endMonth++;
                    }
                }
                else
                {
                    endDay = 30;
                }
            }

            // Calculate total days
            return (endYear * 360 + endMonth * 30 + endDay) - (startYear * 360 + startMonth * 30 + startDay);
        }

        /// <summary>
        /// Checks if the given date is the last day of February.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static bool IsLastDayOfFebruary(DateTime date)
        {
            if (date.Month != 2) return false;

            int last_day_of_february = DateTime.DaysInMonth(date.Year, 2);
            return date.Day == last_day_of_february;
        }
    }


    /// <summary>
    /// Checks if the year of the given date is a leap year.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsLeapYear(this DateTime date)
    {
        return DateTime.IsLeapYear(date.Year);
    }

    /// <summary>
    /// Returns the number of days in the month of the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int GetDaysInMonth(this DateTime date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    /// Returns the number of days in the year of the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int GetDaysInYear(this DateTime date)
    {
        return date.IsLeapYear() ? 366 : 365;
    }

    /// <summary>
    /// Returns the total number of weekdays (Monday to Friday) in the year of the given date.
    /// </summary>
    /// <param name="date_time"></param>
    /// <returns></returns>
    public static int TotalWeekdaysInYear(this DateTime date_time)
    {
        DateTime startOfYear = new DateTime(date_time.Year, 1, 1);
        DateTime endOfYear = new DateTime(date_time.Year, 12, 31);
        int count = 0;

        for (DateTime date = startOfYear; date <= endOfYear; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Converts a DateTime to a string in the format "yyyy-MM-dd T HH-mm-ss".
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToStringFileFormat(this DateTime date)
    {
        return date.ToString(_dateStringFormat);
    }

    /// <summary>
    ///  Checks if date is weekend. If it is weekend it returne the prior or next weekeday based on specifi. This is good for applications where you need a date to be a weekday but you do not know what date will be selected.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="MustBeWeekday"></param>
    /// <returns></returns>
    public static DateTime MustBeWeekdayElseMoveForward(this DateTime date, bool MustBeWeekday)
    {
        if (MustBeWeekday == true)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return date.NextWeekday();
                case DayOfWeek.Saturday:
                    return date.NextWeekday();
                default:
                    return date;
            }
        }
        return date;
    }

    /// <summary>
    /// Checks if date is weekend. If it is weekend it returne the prior or next weekeday based on specifi. This is good for applications where you need a date to be a weekday but you do not know what date will be selected.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="MustBeWeekday"></param>
    /// <returns></returns>
    public static DateTime MustBeWeekdayElseMoveBackward(this DateTime date, bool MustBeWeekday)
    {
        if (MustBeWeekday == true)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return date.PreviousWeekday();
                case DayOfWeek.Saturday:
                    return date.PreviousWeekday();
                default:
                    return date;
            }
        }
        return date;
    }

    private static DateTime ConvertToWeekday(this DateTime date, bool MoveBackward)
    {
        if (MoveBackward == true)
        {
            return date.PreviousWeekday();
        }
        return date.NextWeekday();
    }

    /// <summary>
    /// Returns the previous weekday of the date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime PreviousWeekday(this DateTime date)
    {
        return date.AddWeekdays(-1);
    }

    /// <summary>
    /// Returns the next weekday of the date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime NextWeekday(this DateTime date)
    {
        return date.AddWeekdays(1);
    }

    /// <summary>
    /// Adds a specified number of weekdays to the date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="Days"></param>
    /// <returns></returns>
    public static DateTime AddWeekdays(this DateTime date, int Days)
    {
        int AbsoluteValueOfDays = Math.Abs(Days);
        int DaysToAdd = Days / AbsoluteValueOfDays;
        for (int i = 0; i < AbsoluteValueOfDays; i++)
        {
            do
            {
                date = date.AddDays(DaysToAdd);
            }
            while (date.IsWeekend() == true);
            i++;
        }
        return date;
    }

    /// <summary>
    /// Returns the next specified day of the week from the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static DateTime NextDayOfWeekSpecified(this DateTime date, DayOfWeek dayOfWeek)
    {
        for (int i = 0; i < 8; i++)
        {
            if (date.DayOfWeek == dayOfWeek)
            {
                return date;
            }
            else
            {
                date = date.AddDays(1);
            }
        }
        //Default value
        return default(DateTime);
    }

    /// <summary>
    /// Returns the start of the month for the given date, optionally adding months and ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="monthsToAdd"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateTime StartOfMonth(this DateTime date, int monthsToAdd = 0, bool weekday = false)
    {
        date = date.AddMonths(monthsToAdd);
        DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);

        if (weekday && startOfMonth.IsWeekday() == false)
        {
            startOfMonth.AddWeekdays(1);
        }

        return startOfMonth;
    }

    /// <summary>
    /// Returns the end of the month for the given date, optionally adding months and ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="monthsToAdd"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateTime EndOfMonth(this DateTime date, int monthsToAdd = 0, bool weekday = false)
    {
        date = date.AddMonths(monthsToAdd);

        DateTime endOfMonth = new DateTime(date.Year,
                               date.Month,
                               DateTime.DaysInMonth(date.Year, date.Month));

        if (weekday && endOfMonth.IsWeekday() == false)
        {
            endOfMonth = endOfMonth.PreviousWeekday();
        }
        return endOfMonth;
    }

    /// <summary>
    /// Checks if the date is a weekday (Monday to Friday).
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekday(this DateTime date)
    {
        return (date.DayOfWeek != DayOfWeek.Saturday &
               date.DayOfWeek != DayOfWeek.Sunday);
    }

    /// <summary>
    /// Checks if the date is a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekend(this DateTime date)
    {
        return (date.DayOfWeek == DayOfWeek.Saturday ||
               date.DayOfWeek == DayOfWeek.Sunday);

    }

    /// <summary>
    /// Returns the number of weekdays between two dates, excluding the start date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static int WeekdaysFromDate(this DateTime date, DateTime Date)
    {
        return WeekdaysBetweenDates(date, Date);
    }

    /// <summary>
    /// Returns the number of weekdays between two dates, excluding the start date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="Date"></param>
    /// <returns></returns>
    private static int WeekdaysBetweenDates(DateTime date, DateTime Date)
    {
        var dayDifference = (int)Date.Subtract(date).TotalDays;

        if (dayDifference >= 0)
        {
            return Enumerable
            .Range(1, dayDifference)
            .Select(x => date.AddDays(x))
            .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }
        return Enumerable
            .Range(1, Math.Abs(dayDifference))
            .Select(x => date.AddDays(-x))
            .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);

    }

    /// <summary>
    /// Returns 1 for the first half and 2 for the second half.
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static int HalfYear(this DateTime Date)
    {
        if (Date.Month <= 6)
        {
            return 1;
        }
        return 2;
    }

    /// <summary>
    /// Returns the quarter of the year.
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static int Quarter(this DateTime Date)
    {
        return Convert.ToInt32(Math.Ceiling((decimal)Date.Month / 3));
    }
}
