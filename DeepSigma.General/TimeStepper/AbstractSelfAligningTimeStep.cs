using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Abstract class for self-aligning time step functionality.
/// </summary>
/// <param name="Periodicity"></param>
/// <param name="MustBeWeekday"></param>
public abstract class AbstractSelfAligningTimeStep<T>(Periodicity Periodicity, bool MustBeWeekday = true)
    where T : struct, IDateTime<T>
{
    private protected Periodicity Periodicity { get; init; } = Periodicity;
    private protected bool MustBeWeekday { get; init; } = MustBeWeekday;

    /// <summary>
    /// Returns periodicity from object instance.
    /// </summary>
    /// <returns></returns>
    public Periodicity GetPeriodicity() => Periodicity;

    /// <summary>
    /// Returns next day date time.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <param name="MoveForward"></param>
    /// <returns></returns>
    private protected T GetNextDay(T SelectedDateTime, bool MoveForward = true)
    {
        int Scalar = GetDirectionScalar(MoveForward);
        if (MustBeWeekday) return SelectedDateTime.AddWeekdays(Scalar);
        return SelectedDateTime.AddDays(Scalar);
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

    private protected T GetYearEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidYearEnd(SelectedDateTime) == false)
        {
            return T.Create(new DateTime(SelectedDateTime.Year, 12, 31).MustBeWeekdayElseMoveBackward(MustBeWeekday));
        }
        int YearScalar = GetDirectionScalar(MoveForward);
        return T.Create(new DateTime(SelectedDateTime.Year, 12, 31).AddYears(YearScalar).MustBeWeekdayElseMoveBackward(MustBeWeekday));
    }

    private bool IsValidYearEnd(T SelectedDateTime)
    {
        int Year = SelectedDateTime.Year;
        if (SelectedDateTime.Date.ToDateTime() == new DateTime(Year, 12, 31).MustBeWeekdayElseMoveBackward(MustBeWeekday))
        {
            return true;
        }
        return false;
    }

    private protected T GetMonthEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidMonthEnd(SelectedDateTime) == false)
        {
            return T.Create(new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).EndOfMonth(0, MustBeWeekday));
        }
        int MonthScalar = GetDirectionScalar(MoveForward);
        return T.Create(new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).EndOfMonth(MonthScalar, MustBeWeekday));
    }

    private bool IsValidMonthEnd(T SelectedDateTime)
    {
        int Year = SelectedDateTime.Year;
        int Month = SelectedDateTime.Month;
        if (SelectedDateTime.Date.ToDateTime() == new DateTime(Year, Month, 1).EndOfMonth(0, MustBeWeekday)) return true;
        return false;
    }

    private protected static T GetWeekEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidWeekEnd(SelectedDateTime) == false)
        {
            return SelectedDateTime.NextDayOfWeekSpecified(DayOfWeek.Friday);
        }
        int Scalar = GetDirectionScalar(MoveForward);
        return SelectedDateTime.AddDays(7 * Scalar).NextDayOfWeekSpecified(DayOfWeek.Friday);
    }

    private static bool IsValidWeekEnd(T SelectedDateTime)
    {
        if (SelectedDateTime.DayOfWeek == DayOfWeek.Friday) return true;
        return false;
    }

    private protected T GetQuarterEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        int startingMonths = 3 * SelectedDateTime.Quarter;
        int MonthsToValidEndDate = startingMonths - SelectedDateTime.Month;
        if (MoveForward == true && IsValidQuarterEnd(SelectedDateTime) == false)
        {
            return SelectedDateTime.AddMonths(MonthsToValidEndDate).EndOfMonth(0, MustBeWeekday);
        }
        int Scalar = GetDirectionScalar(MoveForward);
        return SelectedDateTime.AddMonths(MonthsToValidEndDate).AddMonths(Scalar * 3).EndOfMonth(0, MustBeWeekday);
    }

    private bool IsValidQuarterEnd(T SelectedDateTime)
    {
        if (SelectedDateTime.Month % 3 == 0 && SelectedDateTime.Date == SelectedDateTime.EndOfMonth(0, MustBeWeekday).Date)
        {
            return true;
        }
        return false;
    }

    private protected T GetSemiAnnualEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        int startingMonths = 6 * SelectedDateTime.HalfYear;
        int MonthsToValidEndDate = startingMonths - SelectedDateTime.Month;
        if (MoveForward == true && IsValidSemiAnnualEnd(SelectedDateTime) == false)
        {
            return SelectedDateTime.AddMonths(MonthsToValidEndDate).EndOfMonth(0, MustBeWeekday);
        }
        int Scalar = GetDirectionScalar(MoveForward);
        return SelectedDateTime.AddMonths(MonthsToValidEndDate).AddMonths(Scalar * 6).EndOfMonth(0, MustBeWeekday);
    }

    private bool IsValidSemiAnnualEnd(T SelectedDateTime)
    {
        int Year = SelectedDateTime.Year;
        HashSet<T> SemiAnnualEndDates = [
            T.Create(new DateTime(Year, 6, 30).MustBeWeekdayElseMoveBackward(MustBeWeekday)),
            T.Create(new DateTime(Year, 12, 31).MustBeWeekdayElseMoveBackward(MustBeWeekday))
        ];
        if (SemiAnnualEndDates.Contains(SelectedDateTime)) return true;
        return false;
    }
}
