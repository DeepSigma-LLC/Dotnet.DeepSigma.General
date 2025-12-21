
namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for DateOnly type
/// </summary>
public static class DateOnlyExtension
{
    /// <summary>
    /// Converts a DateOnly to a DateTime at midnight.
    /// </summary>
    /// <param name="date_only"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this DateOnly date_only) => date_only.ToDateTime(TimeOnly.MinValue);

    /// <summary>
    /// Checks if the date is a weekday (Monday to Friday).
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekday(this DateOnly date) => !date.IsWeekend();

    /// <summary>
    /// Checks if the date is a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekend(this DateOnly date) => (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

    /// <summary>
    /// Checks if the year of the given date is a leap year.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsLeapYear(this DateOnly date) => DateTime.IsLeapYear(date.Year);

    /// <summary>
    /// Returns the number of days in the month of the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int GetDaysInMonth(this DateOnly date) => DateTime.DaysInMonth(date.Year, date.Month);

    /// <summary>
    /// Returns the number of days in the year of the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int GetDaysInYear(this DateOnly date) => date.IsLeapYear() ? 366 : 365;

    /// <summary>
    /// Returns the previous weekday of the date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateOnly PreviousWeekday(this DateOnly date) => date.AddWeekdays(-1);

    /// <summary>
    /// Returns the next weekday of the date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateOnly NextWeekday(this DateOnly date) => date.AddWeekdays(1);

    /// <summary>
    /// Returns the date itself if it's a weekday; otherwise, returns the next weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateOnly WeekdayOrNext(this DateOnly date) => date.IsWeekend() ? date.NextWeekday() : date;

    /// <summary>
    /// Returns the date itself if it's a weekday; otherwise, returns the previous weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateOnly WeekdayOrPrevious(this DateOnly date) => date.IsWeekend() ? date.PreviousWeekday() : date;

    /// <summary>
    /// Returns the start of the month for the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="months_to_add"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly StartOfMonth(this DateOnly date, int months_to_add = 0, bool weekday = false)
    {
        date = date.AddMonths(months_to_add);
        DateOnly result = new(date.Year, date.Month, 1);
        return weekday ? result.WeekdayOrNext() : result;
    }

    /// <summary>
    /// Returns the end of the month for the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="months_to_add"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly EndOfMonth(this DateOnly date, int months_to_add = 0, bool weekday = false)
    {
        date = date.AddMonths(months_to_add);
        DateOnly result = new(date.Year, date.Month, date.GetDaysInMonth());
        return weekday ? result.WeekdayOrPrevious() : result;
    }
    
    /// <summary>
    /// Adds a specified number of weekdays to the date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="Days"></param>
    /// <returns></returns>
    public static DateOnly AddWeekdays(this DateOnly date, int Days)
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
    /// Returns 1 for the first half and 2 for the second half.
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static int HalfYear(this DateOnly Date) => Date.Month <= 6 ? 1 : 2;

    /// <summary>
    /// Returns the quarter of the year.
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static int Quarter(this DateOnly Date)
    {
        return Convert.ToInt32(Math.Ceiling((decimal)Date.Month / 3));
    }

    /// <summary>
    /// Returns the number of weekdays between two dates, excluding the start date.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static int WeekdaysFromDate(this DateOnly date, DateOnly Date) => WeekdaysBetweenDates(date, Date);

    /// <summary>
    /// Returns the number of weekdays between two dates, excluding the start date.
    /// </summary>
    /// <param name="start_date"></param>
    /// <param name="end_date"></param>
    /// <returns></returns>
    private static int WeekdaysBetweenDates(DateOnly start_date, DateOnly end_date)
    {
        int dayDifference = (int)(end_date.ToDateTime() - start_date.ToDateTime()).TotalDays;

        if (dayDifference >= 0)
        {
            return Enumerable
            .Range(1, dayDifference)
            .Select(x => start_date.AddDays(x))
            .Count(x => x.IsWeekday());
        }
        return Enumerable
            .Range(1, Math.Abs(dayDifference))
            .Select(x => start_date.AddDays(-x))
            .Count(x => x.IsWeekday());
    }

    /// <summary>
    /// Returns the start of the year for the given date, optionally ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly StartOfYear(this DateOnly date, bool weekday = false)
    {
        DateOnly startOfYear = new(date.Year, 1, 1);
        return weekday ? startOfYear.WeekdayOrNext() : startOfYear;
    }

    /// <summary>
    /// Returns the end of the year for the given date, optionally ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly EndOfYear(this DateOnly date, bool weekday = false)
    {
        DateOnly endOfYear = new(date.Year, 12, 31);
        return weekday ? endOfYear.WeekdayOrPrevious() : endOfYear;
    }

    /// <summary>
    /// Returns the start of the quarter for the given date, optionally ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly StartOfQuarter(this DateOnly date, bool weekday = false)
    {
        int month = (date.Quarter() - 1) * 3 + 1;
        DateOnly startOfQuarter = new(date.Year, month, 1);
        return weekday ? startOfQuarter.WeekdayOrNext() : startOfQuarter;
    }

    /// <summary>
    /// Returns the end of the quarter for the given date, optionally ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly EndOfQuarter(this DateOnly date, bool weekday = false)
    {
        int month = (date.Quarter() * 3);
        DateOnly endOfQuarter = new(date.Year, month, DateTime.DaysInMonth(date.Year, month));
        return weekday ? endOfQuarter.WeekdayOrPrevious() : endOfQuarter;
    }

    /// <summary>
    /// Returns the start of the half-year for the given date, optionally ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly StartOfHalfYear(this DateOnly date, bool weekday = false)
    {
        int month = (date.HalfYear() - 1) * 6 + 1;
        DateOnly startOfHalfYear = new(date.Year, month, 1);
        return weekday ? startOfHalfYear.WeekdayOrNext() : startOfHalfYear;
    }

    /// <summary>
    /// Returns the end of the half-year for the given date, optionally ensuring it is a weekday.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static DateOnly EndOfHalfYear(this DateOnly date, bool weekday = false)
    {
        int month = date.HalfYear() * 6;
        DateOnly endOfHalfYear = new(date.Year, month, DateTime.DaysInMonth(date.Year, month));
        return weekday ? endOfHalfYear.WeekdayOrPrevious() : endOfHalfYear;
    }

    /// <summary>
    /// Counts the occurrences of a specific day of the week between two dates, inclusive.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="day_of_week"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static int CountDaysOfWeekBetweenDates(this DateOnly start, DateOnly end, DayOfWeek day_of_week)
    {
        if (end < start) throw new ArgumentException("End date must be >= start date.");

        // Find the first DayOfWeek on or after start
        int daysUntilDayOfWeek = ((int)day_of_week - (int)start.DayOfWeek + 7) % 7;
        DateOnly firstDayOfWeek = start.AddDays(daysUntilDayOfWeek);

        if (firstDayOfWeek > end) return 0;

        // Find the last DayOfWeek on or before end
        int daysSinceDayOfWeek = ((int)end.DayOfWeek - (int)day_of_week + 7) % 7;
        DateOnly lastDayOfWeek = end.AddDays(-daysSinceDayOfWeek);

        int daysBetweenLastAndFirst = lastDayOfWeek.DayNumber - firstDayOfWeek.DayNumber;
        return (daysBetweenLastAndFirst / 7) + 1;
    }
}
