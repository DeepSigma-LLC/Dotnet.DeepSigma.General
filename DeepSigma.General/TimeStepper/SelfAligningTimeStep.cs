using DeepSigma.General.DateObjects;
using DeepSigma.General.Enums;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Self-aligning time step for DateOnly.
/// </summary>
/// <param name="Periodicity"></param>
/// <param name="MustBeWeekday"></param>
public class SelfAligningTimeStep<T>(Periodicity Periodicity, bool MustBeWeekday = true) 
    : AbstractSelfAligningTimeStep<T>(Periodicity, MustBeWeekday)
    where T : struct, IDateTime<T>
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
    public bool IsValidTimeStep(T EvaluationDateTime)
    {
        if (EvaluationDateTime == CalculateTimeStep(EvaluationDateTime, false)) return true;
        return false;
    }

    private T CalculateTimeStep(T SelectedDateTime, bool MoveForward = true)
    {
        T result = Periodicity switch
        {
            Periodicity.Daily => GetNextDay(SelectedDateTime, MoveForward),
            Periodicity.Monthly => GetMonthEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Weekly => GetWeekEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Quarterly => GetQuarterEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Annually => GetYearEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.SemiAnnual => GetSemiAnnualEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Intraday => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
        return result;
    }
}
