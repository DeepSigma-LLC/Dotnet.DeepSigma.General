using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Extensions
{
    /// <summary>
    /// Extension methods for DateTime operations.
    /// </summary>
    public static class DateTimeExtension
    {
        private static string _dateStringFormat { get; } = "MM-dd-yyyy 'T 'HH-mm-ss";
        public static string ToStringFileFormat(this DateTime date)
        {
            return date.ToString(_dateStringFormat);
        }

        /// <summary>
        /// Checks if date is weekend. If it is weekend it returne the prior or next weekeday based on specifi. This is good for applications where you need a date to be a weekday but you do not know what date will be selected.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MustBeWeekdayMoveForward(this DateTime date, bool MustBeWeekday)
        {
            if(MustBeWeekday == true)
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
        /// <returns></returns>
        public static DateTime MustBeWeekdayMoveBackward(this DateTime date, bool MustBeWeekday)
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
            for(int i=0; i<AbsoluteValueOfDays; i++)
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
            
            if(weekday && startOfMonth.IsWeekday() == false)
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

            if(dayDifference >= 0)
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
            if(Date.Month <= 6)
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
}
