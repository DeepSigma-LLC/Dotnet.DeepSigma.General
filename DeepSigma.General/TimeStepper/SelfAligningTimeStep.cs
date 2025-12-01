using DeepSigma.General.Extensions;
using DeepSigma.General.Enums;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// A class that provides functionality to align date times based on specified periodicity and time intervals.
/// </summary>
public class SelfAligningTimeStep(Periodicity Periodicity, TimeInterval Interval, bool MustBeWeekday = true) 
    : AbstractSelfAligningTimeStep(Periodicity, MustBeWeekday), ISelfAligningTimeStep<DateTime>
{
    private TimeInterval TimeInterval { get; init; } = Interval;

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

    /// <summary>
    /// Checks if the provided date time is a valid time step based on the periodicity and time interval.
    /// </summary>
    /// <param name="EvaluationDateTime"></param>
    /// <returns></returns>
    public bool IsValidTimeStep(DateTime EvaluationDateTime)
    {
        if (EvaluationDateTime == CalculateTimeStep(EvaluationDateTime, false)) return true;
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
        HashSet<DateTime> results = [];
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
        return Periodicity switch
        {
            Periodicity.Daily => GetNextDay(SelectedDateTime, MoveForward),
            Periodicity.Intraday => GetNextIntradayDateTime(SelectedDateTime, MoveForward),
            Periodicity.Monthly => GetMonthEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Weekly => GetWeekEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Quarterly => GetQuarterEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.Annually => GetYearEndDateTime(SelectedDateTime, MoveForward),
            Periodicity.SemiAnnual => GetSemiAnnualEndDateTime(SelectedDateTime, MoveForward),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Removes unneeded precision from a date time.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <returns></returns>
    private DateTime GetCleanedDateTime(DateTime SelectedDateTime)
    {
        return Periodicity switch
        {
            Periodicity.Intraday => SelectedDateTime.Date.AddHours(SelectedDateTime.Hour).AddMinutes(SelectedDateTime.Minute),
            _ => SelectedDateTime.Date,
        };
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
    private protected DateTime GetNextIntradayDateTime(DateTime SelectedDateTime, bool MoveForward = true)
    {
        DateTime result;
        if (MoveForward == true)
        {
            result = NextIntradayDateTime(SelectedDateTime);
            if (MustBeWeekday && result.IsWeekend() == true)
            {
                return result.MustBeWeekdayElseMoveForward(MustBeWeekday).Date;
            }
            return result;
        }
        else
        {
            result = PriorIntradayDateTime(SelectedDateTime);
            if (MustBeWeekday == true && result.IsWeekend() == true)
            {
                result = result.MustBeWeekdayElseMoveBackward(MustBeWeekday).Date.AddHours(24);
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
}
