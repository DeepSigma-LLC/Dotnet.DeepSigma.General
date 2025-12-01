using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Self-aligning time step for DateOnly.
/// </summary>
/// <param name="Periodicity"></param>
/// <param name="MustBeWeekday"></param>
public class SelfAligningTimeStepDateOnly(Periodicity Periodicity, bool MustBeWeekday = true) 
    : AbstractSelfAligningTimeStep(Periodicity, MustBeWeekday), ISelfAligningTimeStep<DateOnly>
{
    /// <inheritdoc/>
    public HashSet<DateOnly> GetDateTimes(DateOnly StartDate, DateOnly EndDate, bool IncludeStartAndEndDates = true)
    {
        HashSet<DateOnly> results = [];
        if (IncludeStartAndEndDates == true)
        {
            results.Add(StartDate);
            results.Add(EndDate);
        }

        DateOnly EvaluationDateTime = GetNextTimeStep(StartDate);
        while (EvaluationDateTime < EndDate)
        {
            results.Add(EvaluationDateTime);
            EvaluationDateTime = GetNextTimeStep(EvaluationDateTime);
        }
        return results;
    }

    /// <inheritdoc/>
    public DateOnly GetNextTimeStep(DateOnly SelectedDateTime)
    {
        return CalculateTimeStep(SelectedDateTime, true);
    }

    /// <inheritdoc/>
    public DateOnly GetPreviousTimeStep(DateOnly SelectedDateTime)
    {
        return CalculateTimeStep(SelectedDateTime, false);
    }

    /// <inheritdoc/>
    public bool IsValidTimeStep(DateOnly EvaluationDateTime)
    {
        if (EvaluationDateTime == CalculateTimeStep(EvaluationDateTime, false)) return true;
        return false;
    }

    private DateOnly CalculateTimeStep(DateOnly SelectedDateTime, bool MoveForward = true)
    {
        DateTime result = Periodicity switch
        {
            Periodicity.Daily => GetNextDay(SelectedDateTime.ToDateTime(), MoveForward),
            Periodicity.Monthly => GetMonthEndDateTime(SelectedDateTime.ToDateTime(), MoveForward),
            Periodicity.Weekly => GetWeekEndDateTime(SelectedDateTime.ToDateTime(), MoveForward),
            Periodicity.Quarterly => GetQuarterEndDateTime(SelectedDateTime.ToDateTime(), MoveForward),
            Periodicity.Annually => GetYearEndDateTime(SelectedDateTime.ToDateTime(), MoveForward),
            Periodicity.SemiAnnual => GetSemiAnnualEndDateTime(SelectedDateTime.ToDateTime(), MoveForward),
            Periodicity.Intraday => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
        return result.ToDateOnly();
    }


}
