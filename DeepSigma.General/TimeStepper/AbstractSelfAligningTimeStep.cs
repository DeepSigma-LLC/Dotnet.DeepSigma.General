using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Abstract class for self-aligning time step functionality.
/// </summary>
/// <param name="PeriodicityConfig"></param>
public abstract class AbstractSelfAligningTimeStep<T>(PeriodicityConfiguration PeriodicityConfig)
    where T : struct, IDateTime<T>, IComparable<T>
{
    private protected PeriodicityConfiguration PeriodicityConfig { get; init; } = PeriodicityConfig;
    private protected bool MustBeWeekday { get; init; } = PeriodicityConfig.DayType == DaySelectionType.WeekdaysOnly;

    /// <summary>
    /// Returns periodicity from object instance.
    /// </summary>
    /// <returns></returns>
    public PeriodicityConfiguration GetPeriodicityConfiguration() => PeriodicityConfig;

    /// <summary>
    /// Returns next day date time.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <param name="MoveForward"></param>
    /// <returns></returns>
    private protected T GetNextDay(T SelectedDateTime, bool MoveForward = true)
    {
        int Scalar = GetDirectionScalar(MoveForward);
        return MustBeWeekday ? SelectedDateTime.AddWeekdays(Scalar) : SelectedDateTime.AddDays(Scalar);
    }

    /// <summary>
    /// Returns +1 when Move forward = true and -1 when Move foward = false.
    /// </summary>
    /// <param name="MoveForward"></param>
    /// <returns></returns>
    private static sbyte GetDirectionScalar(bool MoveForward = true) => MoveForward ? (sbyte)1 : (sbyte)-1;
    
    private protected T GetYearEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidYearEnd(SelectedDateTime) == false)
        {
            DateTime result = new(SelectedDateTime.Year, 12, 31);
            return MustBeWeekday ? result.WeekdayOrPrevious() : result;
        }
        int YearScalar = GetDirectionScalar(MoveForward);
        DateTime output = new DateTime(SelectedDateTime.Year, 12, 31).AddYears(YearScalar);
        return MustBeWeekday ? output.WeekdayOrPrevious() : output;
    }

    private bool IsValidYearEnd(T SelectedDateTime)
    {
        DateTime datetime = new(SelectedDateTime.Year, 12, 31);
        return MustBeWeekday ? SelectedDateTime.Date.ToDateTime() == datetime.WeekdayOrPrevious() : SelectedDateTime.Date.ToDateTime() == datetime;
    }

    private protected T GetMonthEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        if (MoveForward == true && IsValidMonthEnd(SelectedDateTime) == false)
        {
            return new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).EndOfMonth(0, MustBeWeekday);
        }
        int MonthScalar = GetDirectionScalar(MoveForward);
        return new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).EndOfMonth(MonthScalar, MustBeWeekday);
    }

    private bool IsValidMonthEnd(T SelectedDateTime)
    {
        int Year = SelectedDateTime.Year;
        int Month = SelectedDateTime.Month;
        return SelectedDateTime.Date.ToDateTime() == new DateTime(Year, Month, 1).EndOfMonth(0, MustBeWeekday);
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

    private static bool IsValidWeekEnd(T SelectedDateTime) => SelectedDateTime.DayOfWeek == DayOfWeek.Friday;

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

    private bool IsValidQuarterEnd(T SelectedDateTime) => SelectedDateTime.Month % 3 == 0 && 
        SelectedDateTime.Date == SelectedDateTime.EndOfMonth(0, MustBeWeekday).Date;

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
        HashSet<T> SemiAnnualEndDates = [new DateTime(Year, 6, 30),  new DateTime(Year, 12, 31)];

        HashSet<T> FinalDates = MustBeWeekday ? 
            SemiAnnualEndDates.Select(d => (T)d.DateTime.WeekdayOrPrevious()).ToHashSet() 
            : SemiAnnualEndDates;

        return FinalDates.Contains(SelectedDateTime);
    }
}
