using DeepSigma.General.Extensions;

namespace DeepSigma.General.DateTimeUnification;

/// <summary>
/// Represents a custom date object that encapsulates a DateOnly value.
/// </summary>
/// <param name="date_time"></param>
public readonly struct DateOnlyCustom(DateOnly date_time) : IDateTime<DateOnlyCustom>
{
    private readonly DateOnly _dateTime = date_time;

    /// <inheritdoc cref="DateOnlyCustom"/>
    public DateOnlyCustom(int year, int month, int day) : this(new DateOnly(year, month, day)) {}

    /// <inheritdoc cref="DateOnlyCustom"/>
    public DateOnlyCustom(DateTime dateTime) : this(dateTime.ToDateOnly()) { }

    /// <inheritdoc/>
    public static DateOnlyCustom Create(DateTime date_time) => new(date_time);

    /// <inheritdoc/>
    public DateTime DateTime => _dateTime.ToDateTime();

    /// <inheritdoc/>
    public DateTimeOffset DateTimeOffset => new(DateTime);

    /// <inheritdoc/>
    public DateOnly DateOnly  => _dateTime;

    /// <inheritdoc/>
    public int Year => _dateTime.Year;

    /// <inheritdoc/>
    public int Month => _dateTime.Month;

    /// <inheritdoc/>
    public int Day => _dateTime.Day;

    /// <inheritdoc/>
    public static DateOnlyCustom MaxValue => new(DateOnly.MaxValue);

    /// <inheritdoc/>
    public static DateOnlyCustom MinValue => new(DateOnly.MinValue);

    /// <inheritdoc/>
    public static DateOnlyCustom Now => new(DateTime.Today);

    /// <inheritdoc/>
    public static DateOnlyCustom NowUtc => new(DateTime.UtcNow);

    /// <inheritdoc/>
    public static DateOnlyCustom Today => new(DateTime.Today);

    /// <inheritdoc/>
    public DateOnly Date => _dateTime;

    /// <inheritdoc/>
    public DayOfWeek DayOfWeek => _dateTime.DayOfWeek;

    /// <inheritdoc/>
    public int DayOfYear => _dateTime.DayOfYear;

    /// <inheritdoc/>
    public int DaysInMonth => DateTime.DaysInMonth(Year, Month);

    /// <inheritdoc/>
    public bool IsLeapYear => DateTime.IsLeapYear(Year);

    /// <inheritdoc/>
    public int DaysInYear => DateTimeExtension.GetDaysInYear(DateTime);

    /// <inheritdoc/>
    public int Quarter => DateTimeExtension.Quarter(DateTime);

    /// <inheritdoc/>
    public int HalfYear => DateTimeExtension.HalfYear(DateTime);

    /// <inheritdoc/>
    public DateOnlyCustom AddDays(int value) => new(_dateTime.AddDays(value));

    /// <inheritdoc/>
    public DateOnlyCustom AddMonths(int months) => new(_dateTime.AddMonths(months));

    /// <inheritdoc/>
    public DateOnlyCustom AddYears(int value) => new(_dateTime.AddYears(value));

    /// <inheritdoc/>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = _dateTime.Year;
        month = _dateTime.Month;
        day = _dateTime.Day;
    }

    /// <inheritdoc/>
    public static TimeSpan operator -(DateOnlyCustom dt, DateOnlyCustom cd) => dt.DateTime - cd.DateTime;

    /// <inheritdoc/>
    public static bool operator >(DateOnlyCustom cd1, DateOnlyCustom cd2) => cd1._dateTime > cd2._dateTime;

    /// <inheritdoc/>
    public static bool operator <(DateOnlyCustom cd1, DateOnlyCustom cd2) => cd1._dateTime < cd2._dateTime;

    /// <inheritdoc/>
    public static bool operator >=(DateOnlyCustom cd1, DateOnlyCustom cd2) => cd1._dateTime >= cd2._dateTime;

    /// <inheritdoc/>
    public static bool operator <=(DateOnlyCustom cd1, DateOnlyCustom cd2) => cd1._dateTime <= cd2._dateTime;

    /// <inheritdoc/>
    public static bool operator ==(DateOnlyCustom cd1, DateOnlyCustom cd2) => cd1._dateTime == cd2._dateTime;

    /// <inheritdoc/>
    public static bool operator !=(DateOnlyCustom cd1, DateOnlyCustom cd2) => cd1._dateTime != cd2._dateTime;

    /// <summary>
    /// Returns the string representation of the date in "yyyy-MM-dd" format.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _dateTime.ToString("yyyy-MM-dd");

    /// <inheritdoc/>
    public override int GetHashCode() => _dateTime.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not DateOnlyCustom other) return false;
        return _dateTime.Equals(other._dateTime);
    }

    /// <inheritdoc/>
    public DateOnlyCustom EndOfMonth(int months_to_add = 0, bool must_be_weekday = false)
    {
        return new(DateTimeExtension.EndOfMonth(DateTime, months_to_add, must_be_weekday));
    }

    /// <inheritdoc/>
    public DateOnlyCustom StartOfMonth(int months_to_add = 0, bool must_be_weekday = false)
    {
        return new(DateTimeExtension.StartOfMonth(DateTime, months_to_add, must_be_weekday));
    }

    /// <inheritdoc/>
    public DateOnlyCustom AddWeekdays(int weekdays)
    {
        return new(DateTimeExtension.AddWeekdays(DateTime, weekdays));
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
    public DateOnlyCustom NextDayOfWeekSpecified(DayOfWeek day_of_week)
    {
        return new(DateTimeExtension.NextDayOfWeekSpecified(DateTime, day_of_week));
    }
}
