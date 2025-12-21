
namespace DeepSigma.General.Enums;

/// <summary>
/// Represents the type of day selection for scheduling or filtering purposes.
/// </summary>
public enum DaySelectionType
{
    /// <summary>
    /// Day selection must be a weekday.
    /// </summary>
    WeekdaysOnly,
    /// <summary>
    /// Day selection must be a weekend day.
    /// </summary>
    WeekendsOnly,
    /// <summary>
    /// Day selection is not specified.
    /// </summary>
    AnyDay,
}

