using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Time span stepper for DateTime.
/// </summary>
public class TimeSpanStepper<T> 
    where T : struct, IDateTime<T>
{
    private readonly TimeSpan _time_span_step;

    /// <inheritdoc cref="TimeSpanStepper{T}"/>
    public TimeSpanStepper(TimeInterval time_interval)
    {
        _time_span_step = time_interval.ToTimeSpan();
    }

    /// <inheritdoc cref="TimeSpanStepper{T}"/>
    public TimeSpanStepper(int Hours = 0, int Minutes = 0, int Seconds = 0, int Milliseconds = 0)
    {
        TimeSpan step = new(0, Hours, Minutes, Seconds, Milliseconds);
        if(_time_span_step.TotalMilliseconds == 0) throw new ArgumentException("Time step must be greater than zero.");
        _time_span_step = step;
    }

    /// <summary>
    /// Get the next step from the given DateTime.
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public T GetNext(T from)
    {
        double total_minutes = from.DateTime.TimeOfDay.TotalMinutes;
        double step_minutes = _time_span_step.TotalMinutes;

        double completed_steps = total_minutes / step_minutes;
        completed_steps = Math.Ceiling(completed_steps);

        double target_steps = completed_steps + 1;
        return from.DateTime.Date.AddMinutes(target_steps * step_minutes);
    }


    /// <summary>
    /// Get the previous step from the given DateTime.
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public T GetPrevious(T from)
    {
        double total_minutes = from.DateTime.TimeOfDay.TotalMinutes;
        double step_minutes = _time_span_step.TotalMinutes;

        double completed_steps = total_minutes / step_minutes;
        completed_steps = Math.Ceiling(completed_steps);

        double remainer = total_minutes % step_minutes;
        double target_steps = remainer == 0 ?
             completed_steps - 1
            : completed_steps;
        return from.DateTime.Date.AddMinutes(target_steps * step_minutes);
    }

    /// <summary>
    /// Get steps between two DateTime values including bounds (from_date and end_date).
    /// </summary>
    /// <param name="from_date"></param>
    /// <param name="end_date"></param>
    /// <returns></returns>
    public IEnumerable<T> GetStepsIncludingBounds(T from_date, T end_date)
    {
        HashSet<T> results = [];
        results.Add(from_date);
        results.Add(end_date);

        IEnumerable<T> steps = GetSteps(from_date, end_date);
        foreach (T step in steps)
        {
            results.Add(step);
        }
        return results;
    }

    /// <summary>
    /// Get steps between two DateTime values excluding bounds (from_date and end_date).
    /// </summary>
    /// <param name="from_date"></param>
    /// <param name="end_date"></param>
    /// <returns></returns>
    public IEnumerable<T> GetStepsExcludingBounds(T from_date, T end_date)
    {
        List<T> results = [];
        IEnumerable<T> steps = GetSteps(from_date, end_date);
        foreach (T step in steps)
        {
            if (step != from_date && step != end_date)
            {
                results.Add(step);
            }
        }
        return results;
    }

    /// <summary>
    /// Get steps between two DateTime values (from_date inclusive, end_date inclusive assuming the step matches).
    /// </summary>
    /// <param name="from_date"></param>
    /// <param name="end_date"></param>
    /// <returns></returns>
    public IEnumerable<T> GetSteps(T from_date, T end_date)
    {
        return from_date < end_date ?
            GetStepsForward(from_date, end_date)
            : GetStepsBackward(from_date, end_date);
    }

    /// <summary>
    /// Get steps forward between two DateTime values (from_date inclusive, end_date inclusive assuming the step matches).
    /// </summary>
    /// <param name="from_date"></param>
    /// <param name="end_date"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private IEnumerable<T> GetStepsForward(T from_date, T end_date)
    {
        if (from_date > end_date) throw new ArgumentException("From date must be less than to date.");

        T current = from_date;
        while (current <= end_date)
        {
            yield return current;
            current = GetNext(current);
        }
    }

    /// <summary>
    /// Get steps backward between two DateTime values (from_date inclusive, end_date inclusive assuming the step matches).
    /// </summary>
    /// <param name="from_date"></param>
    /// <param name="end_date"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private IEnumerable<T> GetStepsBackward(T from_date, T end_date)
    {
        if (from_date < end_date) throw new ArgumentException("From date must be greater than to date.");

        T current = from_date;
        while (current >= end_date)
        {
            yield return current;
            current = GetPrevious(current);
        }
    }
}
