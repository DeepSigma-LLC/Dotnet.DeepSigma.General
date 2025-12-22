using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Self-aligning time step for DateOnly.
/// </summary>
/// <param name="PeriodicityConfig"></param>
public class SelfAligningTimeStep<T>(PeriodicityConfiguration PeriodicityConfig) 
    : AbstractSelfAligningTimeStep<T>(PeriodicityConfig)
    where T : struct, IDateTime<T>, IComparable<T>
{
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
    public T GetNextTimeStep(T SelectedDateTime)
    {
        return CalculateTimeStep(SelectedDateTime, true);
    }

    /// <inheritdoc/>
    public T GetPreviousTimeStep(T SelectedDateTime)
    {
        return CalculateTimeStep(SelectedDateTime, false);
    }

    /// <inheritdoc/>
    public bool IsValidTimeStep(T EvaluationDateTime) => EvaluationDateTime == CalculateTimeStep(EvaluationDateTime, false);
    
    private T CalculateTimeStep(T SelectedDateTime, bool MoveForward = true)
    {
        T result = PeriodicityConfig.Periodicity switch
        {
            Periodicity.Daily => GetNextDay(SelectedDateTime, MoveForward),
            Periodicity.Monthly => GetMonthEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Weekly => GetWeekEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Quarterly => GetQuarterEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Annually => GetYearEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.SemiAnnual => GetSemiAnnualEndDateTime(SelectedDateTime, MoveForward),
            _ => throw new NotImplementedException(),
        };
        return result;
    }
}
