using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Abstract class for self-aligning time step functionality.
/// </summary>
/// <param name="Periodicity"></param>
/// <param name="MustBeWeekday"></param>
public abstract class AbstractSelfAligningTimeStep(Periodicity Periodicity, bool MustBeWeekday = true)
{
    private protected Periodicity Periodicity { get; init; } = Periodicity;
    private protected bool MustBeWeekday { get; init; } = MustBeWeekday;

    /// <summary>
    /// Returns periodicity from object instance.
    /// </summary>
    /// <returns></returns>
    public Periodicity GetPeriodicity()
    {
        return Periodicity;
    }

    /// <summary>
    /// Returns next day date time.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <param name="MoveForward"></param>
    /// <returns></returns>
    private protected DateTime GetNextDay(DateTime SelectedDateTime, bool MoveForward = true)
    {
        int Scalar = GetDirectionScalar(MoveForward);
        if (MustBeWeekday) return SelectedDateTime.Date.AddWeekdays(Scalar);
        return SelectedDateTime.Date.AddDays(Scalar);
    }

    /// <summary>
    /// Returns +1 when Move forward = true and -1 when Move foward = false.
    /// </summary>
    /// <param name="MoveForward"></param>
    /// <returns></returns>
    private static sbyte GetDirectionScalar(bool MoveForward = true)
    {
        if (MoveForward) return 1;
        return -1;
    }


    private protected DateTime GetYearEndDateTime(DateTime SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidYearEnd(SelectedDateTime) == false)
        {
            return new DateTime(SelectedDateTime.Year, 12, 31).MustBeWeekdayElseMoveBackward(MustBeWeekday);
        }
        int YearScalar = GetDirectionScalar(MoveForward);
        return new DateTime(SelectedDateTime.Year, 12, 31).AddYears(YearScalar).MustBeWeekdayElseMoveBackward(MustBeWeekday);
    }

    private bool IsValidYearEnd(DateTime SelectedDateTime)
    {
        int Year = SelectedDateTime.Year;
        if (SelectedDateTime.Date == new DateTime(Year, 12, 31).MustBeWeekdayElseMoveBackward(MustBeWeekday))
        {
            return true;
        }
        return false;
    }

    private protected DateTime GetMonthEndDateTime(DateTime SelectedDateTime, bool MoveForward)
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
        if (SelectedDateTime.Date == new DateTime(Year, Month, 1).EndOfMonth(0, MustBeWeekday)) return true;
        return false;
    }

    private protected static DateTime GetWeekEndDateTime(DateTime SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidWeekEnd(SelectedDateTime) == false)
        {
            return SelectedDateTime.NextDayOfWeekSpecified(DayOfWeek.Friday);
        }
        int Scalar = GetDirectionScalar(MoveForward);
        return SelectedDateTime.AddDays(7 * Scalar).NextDayOfWeekSpecified(DayOfWeek.Friday);
    }

    private static bool IsValidWeekEnd(DateTime SelectedDateTime)
    {
        if (SelectedDateTime.DayOfWeek == DayOfWeek.Friday) return true;
        return false;
    }

    private protected DateTime GetQuarterEndDateTime(DateTime SelectedDateTime, bool MoveForward)
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

    private protected DateTime GetSemiAnnualEndDateTime(DateTime SelectedDateTime, bool MoveForward)
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
        HashSet<DateTime> SemiAnnualEndDates = [
            new DateTime(Year, 6, 30).MustBeWeekdayElseMoveBackward(MustBeWeekday),
            new DateTime(Year, 12, 31).MustBeWeekdayElseMoveBackward(MustBeWeekday)
        ];
        if (SemiAnnualEndDates.Contains(SelectedDateTime.Date)) return true;
        return false;
    }
}
