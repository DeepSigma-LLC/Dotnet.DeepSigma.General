using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.TimeStepper;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Provides extension methods for SortedDictionary.
/// </summary>
public static class SortedDictionaryExtension
{
    /// <summary>
    /// Removed invalid dates from dictionary. Dates that do not 
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> RemoveInvalidDates<TDate, TValue>(this SortedDictionary<TDate, TValue> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = TimeStep.IsTimeStepAligned(StartDate) ? StartDate : TimeStep.GetNextTimeStep(StartDate);

        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out TValue? value);
            if (found) results.Add(selectedDateTime, value!); // only add if found therefore safe to suppress null warning. Will only be null if TValue is nullable type.
            selectedDateTime = TimeStep.GetNextTimeStep(selectedDateTime);
        }
        return results;
    }

    /// <summary>
    /// Rolls forward the last known value to fill missing dates in the time series. Required dates determined from periodicity time stepper.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> FillMissingValuesByRollingAndDropExcess<TDate, TValue>(this SortedDictionary<TDate, TValue> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = TimeStep.IsTimeStepAligned(StartDate) ? StartDate : TimeStep.GetNextTimeStep(StartDate);
        TValue lastKnownValue = Data[StartDate];

        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out TValue? new_value);
            if (found) lastKnownValue = new_value!; // only update if found therefore safe to suppress null warning. Will only be null if TValue is nullable type.

            results.Add(selectedDateTime, lastKnownValue);
            selectedDateTime = TimeStep.GetNextTimeStep(selectedDateTime);
        }
        return results;
    }

    /// <summary>
    /// Rolls forward the last known value to fill missing dates in the time series. Time steps are determined by the provided time stepper.
    /// Retains any excess dates that were originally in the data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> FillMissingValuesByRolling<TDate, TValue>(this SortedDictionary<TDate, TValue> Data, SelfAligningTimeStepper<TDate> TimeStep)
    where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue> results = Data.FillMissingValuesByRollingAndDropExcess(TimeStep);

        // Now retain any excess dates that were originally in the data
        Data.ForEach(x => results.TryAdd(x.Key, x.Value));
        return results;
    }

    /// <summary>
    /// Fills missing dates required by the time step with null if no data exists for that date.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue?> FillMissingValuesWithNullAndDropExcess<TDate, TValue>(this SortedDictionary<TDate, TValue> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = TimeStep.IsTimeStepAligned(StartDate) ? StartDate : TimeStep.GetNextTimeStep(StartDate);
        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out TValue? value);
            results.Add(selectedDateTime, value); // value will be null if not found
            selectedDateTime = TimeStep.GetNextTimeStep(selectedDateTime);
        }
        return results;
    }

    /// <summary>
    /// Fills missing dates required by the time step with null if no data exists for that date. Time steps are determined by the provided time stepper.
    /// Retains any excess dates that were originally in the data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue?> FillMissingValuesWithNull<TDate, TValue>(this SortedDictionary<TDate, TValue> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = Data.FillMissingValuesWithNullAndDropExcess(TimeStep);

        // Now retain any excess dates that were originally in the data
        Data.ForEach(x => results.TryAdd(x.Key, x.Value));
        return results;
    }
    /// <summary>
    /// Gets lagged time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="DaysToLag"></param>
    /// <param name="daySelection"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static SortedDictionary<TDate, TValue> LagByDays<TDate, TValue>(this SortedDictionary<TDate, TValue> Data, int DaysToLag, DaySelectionType daySelection = DaySelectionType.Any)
        where TDate : struct, IDateTime<TDate>
    {
        return daySelection switch
        {
            (DaySelectionType.Any) => AddDaysToDateTimes(Data, -DaysToLag),
            (DaySelectionType.Weekday) => AddBusinessDaysToDateTimes(Data, -DaysToLag),
            (DaySelectionType.Weekend) => AddWeekendDaysToDateTimes(Data, -DaysToLag),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Adds days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="DaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, TValue> AddDaysToDateTimes<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int DaysToAdd)
        where TDate : struct, IDateTime<TDate>
    {
        return DaysToAdd == 0 ? Data :
            Data.ToDictionary(x => x.Key.AddDays(DaysToAdd), x => x.Value).ToSortedDictionary();
    }

    /// <summary>
    /// Adds business days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="BusinessDaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, TValue> AddBusinessDaysToDateTimes<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int BusinessDaysToAdd)
        where TDate : struct, IDateTime<TDate>
    {
        return BusinessDaysToAdd == 0 ? Data :
             Data.ToDictionary(x => x.Key.AddWeekdays(BusinessDaysToAdd), x => x.Value).ToSortedDictionary();
    }

    /// <summary>
    /// Adds business days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="WeekendDaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, TValue> AddWeekendDaysToDateTimes<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int WeekendDaysToAdd)
        where TDate : struct, IDateTime<TDate>
    {
        return WeekendDaysToAdd == 0 ? Data :
             Data.ToDictionary(x => x.Key.AddWeekendDays(WeekendDaysToAdd), x => x.Value).ToSortedDictionary();
    }

}
