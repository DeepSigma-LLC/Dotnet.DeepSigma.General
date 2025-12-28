
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.TimeStepper;
using DeepSigma.General.Utilities;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for Dictionary operations.
/// </summary>
public static class DictionaryExtension
{
    /// <summary>
    /// Converts a Dictionary to a SortedDictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static SortedDictionary<T, Z> ToSortedDictionary<T, Z>(this Dictionary<T, Z> dictionary) 
        where T : notnull
    {
        return new SortedDictionary<T, Z>(dictionary);
    }

    /// <summary>
    /// Removes entries with null values from the dictionary.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static Dictionary<K, V?> RemoveNulls<K, V>(this IDictionary<K, V?> dictionary)
    where K : notnull
    {
        return dictionary.Where(kvp => kvp.Value is not null)
                          .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Keeps only the dates that are valid according to the time stepper, removing any dates not aligned with the time step.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue?> RemoveInvalidDates<TDate, TValue>(this SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = StartDate;
        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out TValue? value);
            if (found)
            {
                results.Add(selectedDateTime, value);
            }
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
    public static SortedDictionary<TDate, TValue?> FillMissingValuesByRollingAndDropExcess<TDate, TValue>(this SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = StartDate;
        TValue? lastKnownValue = default;
        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out TValue? new_value);
            if (found)
            {
                lastKnownValue = new_value;
            }
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
    public static SortedDictionary<TDate, TValue?> FillMissingValuesByRolling<TDate, TValue>(this SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
    where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = Data.FillMissingValuesByRollingAndDropExcess(TimeStep);

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
    public static SortedDictionary<TDate, TValue?> FillMissingValuesWithNullAndDropExcess<TDate, TValue>(this SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = StartDate;
        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out TValue? value);
            results.Add(selectedDateTime, value);
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
    public static SortedDictionary<TDate, TValue?> FillMissingValuesWithNull<TDate, TValue>(this SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
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
    public static SortedDictionary<TDate, TValue?> LagByDays<TDate, TValue>(this SortedDictionary<TDate, TValue?> Data, int DaysToLag, DaySelectionType daySelection = DaySelectionType.Any)
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


    /// <summary>
    /// Retrieves a single series of data from the data set based on a specified target property.
    /// For example, if <typeparamref name="TDataModel"/> has a property named "Price", you can retrieve a series of prices by passing an expression that points to that property.
    /// 
    /// <code>
    /// Dictionary&lt;DateTime, decimal?&gt; priceSeries = dataSet.GetExtractedPropertyAsSeries(x => x.Price);
    /// // Or for a sorted series:
    /// SortedDictionary&lt;DateTime, decimal?&gt; priceSeries = dataSet.GetExtractedPropertyAsSeriesSorted(x => x.Price);
    /// </code>
    /// </summary>
    /// <remarks>
    /// Note: This method uses expressions to specify the target property, allowing for strong typing and compile-time checking of property names.
    /// This method will create a new Dictionary containing the keys from the original data set and the values obtained by evaluating the target property on each <typeparamref name="TDataModel"/> instance.
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TDataModel"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="data"></param>
    /// <param name="target_property"></param>
    /// <returns></returns>
    public static Dictionary<TKey, TResult?> GetExtractedPropertyAsSeries<TKey, TDataModel, TResult>(this IDictionary<TKey, TDataModel> data, Expression<Func<TDataModel, TResult>> target_property)
    where TKey : notnull, IComparable<TKey>
    {
        Func<TDataModel, TResult?> compiled_function = ObjectUtilities.ExpressionToExecutableFunction(target_property);

        return data.ToDictionary(
            kvp => kvp.Key,
            kvp => compiled_function(kvp.Value));
    }

    /// <summary>
    /// Tries to get a value from the dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static (bool found, TValue? value) TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        ArgumentNullException.ThrowIfNull(dict);
        return dict.TryGetValue(key, out var value) ? (true, value) : (false, default);
    }

    /// <summary>
    /// Increments the integer value associated with the specified key in the dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="amount"></param>
    public static void Increment<TKey>(this IDictionary<TKey, int> dict, TKey key, int amount = 1)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] += amount; 
            return;
        }         
        dict[key] = amount;
    }

    /// <summary>
    /// Converts the dictionary to a readable string format.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static string ToReadableString<TKey, TValue>(this IDictionary<TKey, TValue> dict)
    {
        return "{" + string.Join(", ", dict.Select(kvp => $"{kvp.Key}: {kvp.Value}")) + "}";
    }

    /// <summary>
    /// Gets the value associated with the specified key, or adds the key with the provided value if it does not exist.
    /// </summary>
    /// <remarks>
    /// This method uses a hyper-optimized approach with CollectionsMarshal and ref returns for performance.
    /// The implementation improves performance by reducing overhead associated with traditional dictionary access methods.
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue? GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue? value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        if(exists) return val;
        val = value;
        return val;
    }

    /// <summary>
    /// Tries to update the value associated with the specified key in the dictionary. 
    /// Returns true if the update was successful, false if the key does not exist.
    /// </summary>
    /// <remarks>
    /// This method uses a hyper-optimized approach with CollectionsMarshal and ref returns for performance.
    /// The implementation improves performance by reducing overhead associated with traditional dictionary access methods.
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
        if (Unsafe.IsNullRef(ref val)) return false;
        val = value;
        return true;
    }
}
