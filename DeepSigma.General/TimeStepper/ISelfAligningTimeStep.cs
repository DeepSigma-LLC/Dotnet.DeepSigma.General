using DeepSigma.General.Enums;

namespace DeepSigma.General.TimeStepper;

/// <summary>
/// Interface for self-aligning time steps.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISelfAligningTimeStep<T> where T : struct, IComparable<T>
{
    /// <summary>
    /// Gets a set of date times between the specified start and end dates.
    /// </summary>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="IncludeStartAndEndDates"></param>
    /// <returns></returns>
    HashSet<T> GetDateTimes(T StartDate, T EndDate, bool IncludeStartAndEndDates = true);

    /// <summary>
    /// Gets the next time step from the selected date time.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <returns></returns>
    T GetNextTimeStep(T SelectedDateTime);


    /// <summary>
    /// Gets the previous time step from the selected date time.
    /// </summary>
    /// <param name="SelectedDateTime"></param>
    /// <returns></returns>
    T GetPreviousTimeStep(T SelectedDateTime);

    /// <summary>
    /// Gets the periodicity of the time step.
    /// </summary>
    /// <returns></returns>
    Periodicity GetPeriodicity();

    /// <summary>
    /// Determines whether the specified evaluation date time is a valid time step.
    /// </summary>
    /// <param name="EvaluationDateTime"></param>
    /// <returns></returns>
    bool IsValidTimeStep(T EvaluationDateTime);
}