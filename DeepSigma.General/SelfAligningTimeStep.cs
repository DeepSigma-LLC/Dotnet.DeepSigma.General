using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.General.Extensions;

namespace DeepSigma.General.Enums
{
    public class SelfAligningTimeStep
    {
        private Periodicity Periodicity { get; set; }
        private TimeInterval TimeInterval { get; set; }
        private bool MustBeWeekday { get; set; } = false;
        public SelfAligningTimeStep(Periodicity Periodicity, TimeInterval Interval, bool MustBeWeekday = true)
        { 
            this.Periodicity = Periodicity;
            this.TimeInterval = Interval;
            this.MustBeWeekday = MustBeWeekday;
        }

        /// <summary>
        /// Returns periodicity from object instance.
        /// </summary>
        /// <returns></returns>
        public Periodicity GetPeriodicity()
        {
            return Periodicity;
        }

        /// <summary>
        /// Returns time interval from object instance.
        /// </summary>
        /// <returns></returns>
        public TimeInterval GetTimeStepMinuteInterval()
        {
            return TimeInterval;
        }

        /// <summary>
        /// Returns date time of next time step.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <returns></returns>
        public DateTime GetNextTimeStep(DateTime SelectedDateTime)
        {
            return CalculateTimeStep(SelectedDateTime, true);
        }

        /// <summary>
        /// Retruns date time of prior time step.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <returns></returns>
        public DateTime GetPreviousTimeStep(DateTime SelectedDateTime)
        {
            return CalculateTimeStep(SelectedDateTime, false);
        }


        public bool IsValidTimeStep(DateTime EvaluationDateTime)
        {
            if(EvaluationDateTime == CalculateTimeStep(EvaluationDateTime, false))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get targeted date times.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="IncludeStartAndEndDates"></param>
        /// <returns></returns>
        public HashSet<DateTime> GetDateTimes(DateTime StartDate, DateTime EndDate, bool IncludeStartAndEndDates = true)
        {
            HashSet<DateTime> results = new HashSet<DateTime>();
            if (IncludeStartAndEndDates == true)
            {
                results.Add(StartDate);
                results.Add(EndDate);
            }

            DateTime EvaluationDateTime = GetNextTimeStep(StartDate);
            while (EvaluationDateTime < EndDate)
            {
                results.Add(EvaluationDateTime);
                EvaluationDateTime = GetNextTimeStep(EvaluationDateTime);
            }
            return results;
        }


        private DateTime CalculateTimeStep(DateTime SelectedDateTime, bool MoveForward = true)
        {
            SelectedDateTime = GetCleanedDateTime(SelectedDateTime);
            switch (Periodicity)
            {
                case Periodicity.Daily:
                     return GetNextDay(SelectedDateTime, MoveForward);
                case Periodicity.Intraday:
                    return GetNextIntradayDateTime(SelectedDateTime, MoveForward);
                case Periodicity.Monthly:
                    return GetMonthEndDateTime(SelectedDateTime, MoveForward);
                case Periodicity.Weekly:
                    return GetWeekEndDateTime(SelectedDateTime, MoveForward);
                case Periodicity.Quarterly:
                    return GetQuarterEndDateTime(SelectedDateTime, MoveForward);
                case Periodicity.Annually:
                    return GetYearEndDateTime(SelectedDateTime,MoveForward);
                case Periodicity.SemiAnnual:
                    return GetSemiAnnualEndDateTime(SelectedDateTime,MoveForward);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Removes unneeded precision from a date time.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <returns></returns>
        private DateTime GetCleanedDateTime(DateTime SelectedDateTime)
        {
            switch (Periodicity)
            {
                case Periodicity.Intraday:
                    return SelectedDateTime.Date.AddHours(SelectedDateTime.Hour).AddMinutes(SelectedDateTime.Minute);
                default:
                    return SelectedDateTime.Date;
            }
        }

        /// <summary>
        /// Returns next day date time.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <param name="MoveForward"></param>
        /// <returns></returns>
        private DateTime GetNextDay(DateTime SelectedDateTime, bool MoveForward = true)
        {
            int Scalar = GetDirectionScalar(MoveForward);
            if (MustBeWeekday)
            {
                return SelectedDateTime.Date.AddWeekdays(Scalar);
            }
            return SelectedDateTime.Date.AddDays(Scalar);
        }

        /// <summary>
        /// Returns +1 when Move forward = true and -1 when Move foward = false.
        /// </summary>
        /// <param name="MoveForward"></param>
        /// <returns></returns>
        private sbyte GetDirectionScalar(bool MoveForward = true)
        {
            if (MoveForward)
            {
                return 1;
            }
            return -1;
        }

        /// <summary>
        /// Aligns intraday date time with the designated time interval.
        /// Examples:
        /// Assume that the time interval is 5 mins.
        /// If the time is 3:05 and MoveForward = false, then 3:00 is returned.
        /// If the time is 3:05 and MoveForward = true, then 3:10 is returned.
        /// If the time is 3:06 and MoveForward = true, then 3:10 is returned.
        /// If the time is 3:06 and MoveForward = false, then 3:05 is returned.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <param name="MoveForward"></param>
        /// <returns></returns>
        private DateTime GetNextIntradayDateTime(DateTime SelectedDateTime, bool MoveForward = true)
        {
            DateTime result;
            if (MoveForward == true)
            {
                result = NextIntradayDateTime(SelectedDateTime);
                if(MustBeWeekday && result.IsWeekend() == true)
                {
                    return result.MustBeWeekdayMoveForward(MustBeWeekday).Date;
                }
                return result;
            }
            else
            {
                result = PriorIntradayDateTime(SelectedDateTime);
                if (MustBeWeekday == true && result.IsWeekend() == true)
                {
                    result = result.MustBeWeekdayMoveBackward(MustBeWeekday).Date.AddHours(24);
                    return PriorIntradayDateTime(result);
                }
                return result;
            }                
        }

        /// <summary>
        /// Returns next intraday date time.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <returns></returns>
        private DateTime NextIntradayDateTime(DateTime SelectedDateTime)
        {
            int Minutes = SelectedDateTime.Minute;
            int Hours = SelectedDateTime.Hour;
            int TotalMinutes = Hours * 60 + Minutes;
            SelectedDateTime = SelectedDateTime.Date.AddHours(Hours).AddMinutes(Minutes);

            int MinutesPastTimeStep = TotalMinutes % (int)TimeInterval;
            return SelectedDateTime.AddMinutes((int)TimeInterval - MinutesPastTimeStep);
        }

        /// <summary>
        /// Returns prior intraday date time.
        /// </summary>
        /// <param name="SelectedDateTime"></param>
        /// <returns></returns>
        private DateTime PriorIntradayDateTime(DateTime SelectedDateTime)
        {
            int Minutes = SelectedDateTime.Minute;
            int Hours = SelectedDateTime.Hour;
            int TotalMinutes = Hours * 60 + Minutes;
            SelectedDateTime = SelectedDateTime.Date.AddHours(Hours).AddMinutes(Minutes);

            int MinutesPastTimeStep = TotalMinutes % (int)TimeInterval;
            if (MinutesPastTimeStep == 0)
            {
                return SelectedDateTime.AddMinutes(-(int)TimeInterval);
            }
            return SelectedDateTime.AddMinutes(-MinutesPastTimeStep);
        }

        private DateTime GetYearEndDateTime(DateTime SelectedDateTime, bool MoveForward)
        {
            if (MoveForward == true && IsValidYearEnd(SelectedDateTime) == false)
            {
                return new DateTime(SelectedDateTime.Year, 12, 31).MustBeWeekdayMoveBackward(MustBeWeekday);
            }
            int YearScalar = GetDirectionScalar(MoveForward);
            return new DateTime(SelectedDateTime.Year, 12, 31).AddYears(YearScalar).MustBeWeekdayMoveBackward(MustBeWeekday);
        }

        private bool IsValidYearEnd(DateTime SelectedDateTime)
        {
            int Year = SelectedDateTime.Year;
            if (SelectedDateTime.Date == new DateTime(Year, 12, 31).MustBeWeekdayMoveBackward(MustBeWeekday))
            {
                return true;
            }
            return false;
        }

        private DateTime GetMonthEndDateTime(DateTime SelectedDateTime, bool MoveForward)
        {
            if (MoveForward == true && IsValidMonthEnd(SelectedDateTime) == false)
            {
                return new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).EndOfMonth(0, MustBeWeekday);
            }
            int MonthScalar = GetDirectionScalar(MoveForward);
            return new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).EndOfMonth(MonthScalar, MustBeWeekday);
        }

        private bool IsValidMonthEnd(DateTime SelectedDateTime)
        {
            int Year = SelectedDateTime.Year;
            int Month = SelectedDateTime.Month;
            if (SelectedDateTime.Date == new DateTime(Year, Month, 1).EndOfMonth(0, MustBeWeekday))
            {
                return true;
            }
            return false;
        }

        private DateTime GetWeekEndDateTime(DateTime SelectedDateTime, bool MoveForward)
        {
            if (MoveForward == true && IsValidWeekEnd(SelectedDateTime) == false)
            {
                return SelectedDateTime.NextDayOfWeekSpecified(DayOfWeek.Friday);
            }
            int Scalar = GetDirectionScalar(MoveForward);
            return SelectedDateTime.AddDays(7 * Scalar).NextDayOfWeekSpecified(DayOfWeek.Friday);
        }

        private bool IsValidWeekEnd(DateTime SelectedDateTime)
        {
            if (SelectedDateTime.DayOfWeek == DayOfWeek.Friday)
            {
                return true;
            }
            return false;
        }

        private DateTime GetQuarterEndDateTime(DateTime SelectedDateTime, bool MoveForward)
        {
            int startingMonths = 3 * SelectedDateTime.Quarter();
            int MonthsToValidEndDate = startingMonths - SelectedDateTime.Month;
            if (MoveForward == true && IsValidQuarterEnd(SelectedDateTime) == false)
            {
                return SelectedDateTime.AddMonths(MonthsToValidEndDate).EndOfMonth(0, MustBeWeekday);
            }
            int Scalar = GetDirectionScalar(MoveForward);
            return SelectedDateTime.AddMonths(MonthsToValidEndDate).AddMonths(Scalar * 3).EndOfMonth(0, MustBeWeekday);
        }

        private bool IsValidQuarterEnd(DateTime SelectedDateTime)
        {
            if (SelectedDateTime.Month % 3 == 0 && SelectedDateTime.Date == SelectedDateTime.Date.EndOfMonth(0, MustBeWeekday))
            {
                return true;
            }
            return false;
        }

        private DateTime GetSemiAnnualEndDateTime(DateTime SelectedDateTime, bool MoveForward)
        {
            int startingMonths = 6 * SelectedDateTime.HalfYear();
            int MonthsToValidEndDate = startingMonths - SelectedDateTime.Month;
            if (MoveForward == true && IsValidSemiAnnualEnd(SelectedDateTime) == false)
            {
                return SelectedDateTime.AddMonths(MonthsToValidEndDate).EndOfMonth(0, MustBeWeekday);
            }
            int Scalar = GetDirectionScalar(MoveForward);
            return SelectedDateTime.AddMonths(MonthsToValidEndDate).AddMonths(Scalar * 6).EndOfMonth(0, MustBeWeekday);
        }

        private bool IsValidSemiAnnualEnd(DateTime SelectedDateTime)
        {
            int Year = SelectedDateTime.Year;
            if (SelectedDateTime.Date == new DateTime(Year, 6, 30).MustBeWeekdayMoveBackward(MustBeWeekday))
            {
                return true;
            }
            else if (SelectedDateTime.Date == new DateTime(Year, 12, 31).MustBeWeekdayMoveBackward(MustBeWeekday))
            {
                return true;
            }
            return false;
        }
    }
}
