using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Self-aligning time stepper is a utility to calculate time steps based on periodicity configuration.
/// The goal of this class is to provide time steps that align with specific periodicity rules, such as month-end or quarter-end dates.
/// This class supports various periodicities including daily, weekly, monthly, quarterly, semi-annual, and annual.
/// </summary>
/// <remarks>
/// The type parameter T must implement IDateTime{T} IComparable{T} to ensure compatibility with date-time operations and comparisons.
/// The goal is to provide a flexible and reusable component for time step calculations in scheduling or time series applications.
/// </remarks>
/// <typeparam name="T"></typeparam>
public class NewSelfAligningTimeStepper<T>
    where T : struct, IDateTime<T>, IComparable<T>
{
    /// <summary>
    /// Periodicity configuration that defines how time steps are calculated.
    /// </summary>
    private protected PeriodicityConfiguration PeriodicityConfig { get; init; }

    /// <summary>
    /// Indicates whether adjustments should be made forward in time (true) or backward in time (false).
    /// </summary>
    /// <remarks>
    /// For example, if a date falls on a weekend and this is true, it will adjust to the next weekday (assuming weekdays are required).
    /// If false, it will adjust to the previous weekday.
    /// </remarks>
    private protected bool AdjustmentDirectionForward { get; init; }

    /// <summary>
    /// Returns +1 when Move forward = true and -1 when Move foward = false.
    /// </summary>
    /// <param name="MoveForward"></param>
    /// <returns></returns>
    private int GetMoveDirectionScalar(bool MoveForward = true) => MoveForward ? 1 : -1;

    /// <summary>
    /// Time span stepper for intraday time step calculations.
    /// </summary>
    private TimeSpanStepper<T>? TimeSpanStepper { get; init; }

    /// <summary>
    /// Gets the required day of the week for weekly periodicity.
    /// </summary>
    private protected DayOfWeek? RequiredDayOfWeek
    {
        get => field;
        init => field = PeriodicityConfig.DayType == DaySelectionType.SpecificDayOfWeek ? value : null; // Ensures value is only set when DayType is SpecificDayOfWeek
    }

    /// <inheritdoc cref="NewSelfAligningTimeStepper{T}(PeriodicityConfiguration, bool, DayOfWeek?)"/>
    public NewSelfAligningTimeStepper(PeriodicityConfiguration PeriodicityConfig, bool AdjustmentDirectionForward = true, DayOfWeek? required_day_of_week = null)
    {
        ValidateDaySelection(PeriodicityConfig, required_day_of_week);
        this.PeriodicityConfig = PeriodicityConfig;
        this.AdjustmentDirectionForward = AdjustmentDirectionForward;
        this.RequiredDayOfWeek = required_day_of_week;

        if(PeriodicityConfig.Time is not null)
        {
            this.TimeSpanStepper = new(PeriodicityConfig.Time.Value);
        }
    }

    /// <summary>
    /// Validates the day selection configuration.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="required_day_of_week"></param>
    /// <exception cref="ArgumentException"></exception>
    private void ValidateDaySelection(PeriodicityConfiguration config, DayOfWeek? required_day_of_week)
    {
        if (typeof(T) == typeof(DateOnlyCustom) && config.Time is not null)
            throw new ArgumentException($"Time interval is not supported for {nameof(DateOnlyCustom)} type.");

        if (config.DayType == DaySelectionType.SpecificDayOfWeek && required_day_of_week is null)
            throw new ArgumentException("Required day of week must be provided when day selection is specific day of week.");

        if (config.DayType != DaySelectionType.SpecificDayOfWeek && required_day_of_week is not null)
            throw new ArgumentException("Required day of week should be null unless day selection is specific day of week.");
    }

    /// <inheritdoc/>
    public HashSet<T> GetDateTimes(T StartDate, T EndDate, bool IncludeStartAndEndDates = true)
    {
        HashSet<T> results = [];
        if (IncludeStartAndEndDates == true)
        {
            results.Add(StartDate);
            results.Add(EndDate);
        }

        T EvaluationDateTime = GetNextTimeStep(StartDate);
        while (EvaluationDateTime < EndDate)
        {
            results.Add(EvaluationDateTime);
            EvaluationDateTime = GetNextTimeStep(EvaluationDateTime);
        }
        return results;
    }

    /// <inheritdoc/>
    public T GetNextTimeStep(T SelectedDateTime) => CalculateTimeStep(SelectedDateTime, true);
    
    /// <inheritdoc/>
    public T GetPreviousTimeStep(T SelectedDateTime) => CalculateTimeStep(SelectedDateTime, false);

    /// <summary>
    /// Calculates the next or previous time step based on the periodicity configuration.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <param name="step_forward"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private T CalculateTimeStep(T SelectedDateTime, bool step_forward)
    {
        T result = PeriodicityConfig.Periodicity switch
        {
            Periodicity.Daily => IsIntraday() ? GetNextIntradayDateTime(SelectedDateTime, step_forward) :
                AdjustDate(SelectedDateTime, step_forward),
            Periodicity.Monthly => GetMonthEndDateTime(SelectedDateTime, step_forward),
            Periodicity.Weekly => GetWeekEndDateTime(SelectedDateTime, step_forward),
            Periodicity.Quarterly => GetQuarterEndDateTime(SelectedDateTime, step_forward),
            Periodicity.Annually => GetYearEndDateTime(SelectedDateTime, step_forward),
            Periodicity.SemiAnnual => GetSemiAnnualEndDateTime(SelectedDateTime, step_forward),
            _ => throw new NotImplementedException(),
        };
        return result;
    }

    private bool IsIntraday() => PeriodicityConfig.Periodicity == Periodicity.Daily
        && PeriodicityConfig.Time is not null;

    private T GetNextIntradayDateTime(T SelectedDateTime, bool step_forward)
    {
        throw new NotImplementedException("Intraday time stepping is not yet implemented.");
    }


    private T GetWeekEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        return MoveForward ?
            AdjustDateIfNeeded(SelectedDateTime.NextDayOfWeekSpecified(DayOfWeek.Friday))
            : AdjustDateIfNeeded(SelectedDateTime.PreviousDayOfWeekSpecified(DayOfWeek.Friday));
    }

    private T GetMonthEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        DateTime eom_date = SelectedDateTime.DateTime.EndOfMonth();
        int months_to_add = GetMoveDirectionScalar(MoveForward);

        (bool found, T new_date) = ValidDateFound(SelectedDateTime, MoveForward, eom_date);
        if (found) return new_date;

        new_date = eom_date.AddMonths(months_to_add).EndOfMonth();
        return AdjustDateIfNeeded(new_date.AddMonths(months_to_add).EndOfMonth());
    }

    private T GetQuarterEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        DateTime eoq_date = SelectedDateTime.DateTime.EndOfQuarter();
        int months_to_add = GetMoveDirectionScalar(MoveForward) * 3;

        (bool found, T new_date) = ValidDateFound(SelectedDateTime, MoveForward, eoq_date);
        if (found) return new_date;

        new_date = eoq_date.AddMonths(months_to_add).EndOfQuarter();
        return AdjustDateIfNeeded(new_date);
    }

    private T GetSemiAnnualEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        DateTime eos_date = SelectedDateTime.DateTime.EndOfHalfYear();
        int months_to_add = GetMoveDirectionScalar(MoveForward) * 6;

        (bool found, T new_date) = ValidDateFound(SelectedDateTime, MoveForward, eos_date);
        if (found) return new_date;
        
        new_date = eos_date.AddMonths(months_to_add).EndOfHalfYear();
        return AdjustDateIfNeeded(new_date);
    }

    private T GetYearEndDateTime(T SelectedDateTime, bool MoveForward)
    {
        DateTime eoy_date = SelectedDateTime.DateTime.EndOfYear();
        int years_to_add = GetMoveDirectionScalar(MoveForward);

        (bool found, T new_date) = ValidDateFound(SelectedDateTime, MoveForward, eoy_date);
        if (found) return new_date;
        
        new_date = new_date.AddYears(years_to_add);
        return AdjustDateIfNeeded(new_date);
    }

    private (bool found, T value) ValidDateFound(T SelectedDateTime, bool MoveForward, DateTime new_date)
    {
        new_date = AdjustDateIfNeeded(new_date);
        return MoveForward switch
        {
            true
                when new_date > SelectedDateTime.DateTime => (found: true, value: new_date),
            false
                when new_date < SelectedDateTime.DateTime => (found: true, value: new_date),
            _ => (found: true, value: default),
        };
    }

    /// <summary>
    /// Adjusts the SelectedDateTime until it matches the required day type as per PeriodicityConfig.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <returns></returns>
    private T AdjustDateIfNeeded(T SelectedDateTime)
    {
        return IsValidDayOfWeek(SelectedDateTime) ? SelectedDateTime : AdjustDate(SelectedDateTime, AdjustmentDirectionForward);
    }

    /// <summary>
    /// Checks if the SelectedDateTime matches the required day type as per PeriodicityConfig.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private bool IsValidDayOfWeek(T SelectedDateTime)
    {
        return PeriodicityConfig.DayType switch
        {
            DaySelectionType.Any => true,
            DaySelectionType.Weekday => SelectedDateTime.DayOfWeek.IsWeekday(),
            DaySelectionType.Weekend => SelectedDateTime.DayOfWeek.IsWeekend(),
            DaySelectionType.SpecificDayOfWeek => SelectedDateTime.DayOfWeek == RequiredDayOfWeek,
            _ => throw new NotSupportedException($"Day selection type {PeriodicityConfig.DayType} is not supported."),
        };
    }

    /// <summary>
    /// Adjusts the SelectedDateTime to the next valid date based on the DaySelectionType in PeriodicityConfig.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <param name="adjust_forward"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private T AdjustDate(T SelectedDateTime, bool adjust_forward)
    {
        int days_to_add = GetMoveDirectionScalar(adjust_forward);
        return PeriodicityConfig.DayType switch
        {
            DaySelectionType.Any => SelectedDateTime.AddDays(days_to_add),
            DaySelectionType.Weekday => SelectedDateTime.AddWeekdays(days_to_add),
            DaySelectionType.Weekend => SelectedDateTime.AddWeekendDays(days_to_add),
            // null-forgiving operator is safe here due to validation in constructor
            DaySelectionType.SpecificDayOfWeek => AdjustmentDirectionForward
                ? SelectedDateTime.NextDayOfWeekSpecified(RequiredDayOfWeek!.Value)
                : SelectedDateTime.PreviousDayOfWeekSpecified(RequiredDayOfWeek!.Value), 
            _ => throw new NotSupportedException($"Day selection type {PeriodicityConfig.DayType} is not supported."),
        };
    }
}
