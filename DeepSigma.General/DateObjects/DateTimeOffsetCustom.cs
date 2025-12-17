using DeepSigma.General.Extensions; 

namespace DeepSigma.General.DateObjects;

/// <summary>
/// Represents a custom date object that encapsulates a DateTimeOffset value.
/// </summary>
public readonly struct DateTimeOffsetCustom : IDateTime<DateTimeOffsetCustom>
{
    private readonly DateTimeOffset _dateTimeOffset;

    /// <inheritdoc cref="DateTimeOffsetCustom"/>
    DateTimeOffsetCustom(DateTimeOffset dateTimeOffset)
    {
        _dateTimeOffset = dateTimeOffset;
    }

    /// <inheritdoc/>
    public static DateTimeOffsetCustom Create(DateTime date_time) => new(date_time);

    /// <inheritdoc/>
    public static DateTimeOffsetCustom MaxValue => new(DateTimeOffset.MaxValue);

    /// <inheritdoc/>
    public static DateTimeOffsetCustom MinValue => new(DateTimeOffset.MinValue);

    /// <inheritdoc/>
    public static DateTimeOffsetCustom Now => new(DateTimeOffset.Now);

    /// <inheritdoc/>
    public static DateTimeOffsetCustom NowUtc => new(DateTimeOffset.UtcNow);

    /// <inheritdoc/>
    public static DateTimeOffsetCustom Today => new(DateTimeOffset.Now.Date);

    /// <inheritdoc/>
    public DateOnly Date => _dateTimeOffset.DateTime.ToDateOnly();

    /// <inheritdoc/>
    public DateOnly DateOnly => _dateTimeOffset.DateTime.ToDateOnly();

    /// <inheritdoc/>
    public DateTime DateTime => _dateTimeOffset.DateTime;

    /// <inheritdoc/>
    public DateTimeOffset DateTimeOffset => _dateTimeOffset;

    /// <inheritdoc/>
    public int Day => _dateTimeOffset.Day;

    /// <inheritdoc/>
    public DayOfWeek DayOfWeek => _dateTimeOffset.DayOfWeek;

    /// <inheritdoc/>
    public int DayOfYear => _dateTimeOffset.DayOfYear;

    /// <inheritdoc/>
    public int DaysInMonth => DateTime.DaysInMonth(Year, Month);

    /// <inheritdoc/>
    public bool IsLeapYear => DateTime.IsLeapYear(Year);

    /// <inheritdoc/>
    public int Month => _dateTimeOffset.Month;

    /// <inheritdoc/>
    public int Year => _dateTimeOffset.Year;

    /// <inheritdoc/>
    public int DaysInYear => DateTimeExtension.GetDaysInYear(DateTime);

    /// <inheritdoc/>
    public int Quarter => DateTimeExtension.Quarter(DateTime);

    /// <inheritdoc/>
    public int HalfYear => DateTimeExtension.HalfYear(DateTime);

    /// <inheritdoc/>
    public DateTimeOffsetCustom AddDays(int value) => new(_dateTimeOffset.AddDays(value));

    /// <inheritdoc/>
    public DateTimeOffsetCustom AddMonths(int months) => new(_dateTimeOffset.AddMonths(months));

    /// <inheritdoc/>
    public DateTimeOffsetCustom AddWeekdays(int weekdays)
    {
        return new(_dateTimeOffset.DateTime.AddWeekdays(weekdays));
    }

    /// <inheritdoc/>
    public DateTimeOffsetCustom AddYears(int value) => new(_dateTimeOffset.AddYears(value));

    /// <inheritdoc/>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = _dateTimeOffset.Year;
        month = _dateTimeOffset.Month;
        day = _dateTimeOffset.Day;
    }

    /// <inheritdoc/>
    public DateTimeOffsetCustom EndOfMonth(int months_to_add = 0, bool must_be_weekday = false)
    {
        return new(_dateTimeOffset.DateTime.EndOfMonth(months_to_add, must_be_weekday));
    }

    /// <inheritdoc/>
    public bool IsWeekday()
    {
        return DateTimeExtension.IsWeekday(DateTime);
    }

    /// <inheritdoc/>
    public bool IsWeekend()
    {
        return DateTimeExtension.IsWeekend(DateTime);
    }

    /// <inheritdoc/>
    public DateTimeOffsetCustom NextDayOfWeekSpecified(DayOfWeek day_of_week)
    {
        return new(_dateTimeOffset.DateTime.NextDayOfWeekSpecified(day_of_week));
    }

    /// <inheritdoc/>
    public DateTimeOffsetCustom StartOfMonth(int months_to_add = 0, bool must_be_weekday = false)
    {
        return new(_dateTimeOffset.DateTime.StartOfMonth(months_to_add, must_be_weekday));
    }

    /// <inheritdoc/>
    public static TimeSpan operator -(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return left.DateTimeOffset - right.DateTimeOffset;
    }

    /// <inheritdoc/>
    public static bool operator ==(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator !=(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public static bool operator <(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return left < right;
    }

    /// <inheritdoc/>
    public static bool operator >(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return left > right;
    }

    /// <inheritdoc/>
    public static bool operator <=(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return left <= right;
    }

    /// <inheritdoc/>
    public static bool operator >=(DateTimeOffsetCustom left, DateTimeOffsetCustom right)
    {
        return left >= right;
    }
}
