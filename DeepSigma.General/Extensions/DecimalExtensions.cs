using DeepSigma.General.Enums;
using System.Globalization;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Provides extension methods for decimal values, including rounding, formatting, and precision handling.
/// </summary>
public static class DecimalExtensions
{

    /// <summary>
    /// Calculates the percentage of a value relative to a total.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    public static decimal PercentOf(this decimal value, decimal total) => total == 0 ? 0 : (value / total) * 100;

    /// <summary>
    /// Calculates the specified percentage of a decimal value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public static decimal Percent(this decimal value, decimal percent) => (value * percent) / 100;

    /// <summary>
    /// Adds the specified percentage to a decimal value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public static decimal AddPercent(this decimal value, decimal percent)  => value + value.Percent(percent);

    /// <summary>
    /// Subtracts the specified percentage from a decimal value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public static decimal SubtractPercent(this decimal value, decimal percent) => value - value.Percent(percent);

    /// <summary>
    /// Rounds a decimal value based on the specified rounding type.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="roundType"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static decimal Round(this decimal value, RoundType roundType = RoundType.Normal)
    {
        switch (roundType)
        {
            case (RoundType.Normal):
                return Math.Round(value);
            case (RoundType.RoundUp):
                return Math.Ceiling(value);
            case (RoundType.RoundDown):
                return Math.Floor(value);
            case (RoundType.RoundAwayFromZero):
                if (value > 0) { return Math.Ceiling(value); }
                else { return Math.Floor(value); }
            case (RoundType.RoundTowardZero):
                if (value > 0) { return Math.Floor(value); }
                else { return Math.Ceiling(value); }
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Drops unnecessary precision from a decimal value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal DropUnnessaryPrecision(this decimal value)
    {
        return value / 1.000000000000000000000000000000000m;
    }

    /// <summary>
    /// Converts a decimal value to a percentage string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimalCount"></param>
    /// <returns></returns>
    public static string ToPercentage(this decimal value, int decimalCount = 3)
    {
        return value.ToString("P" + decimalCount.ToString());
    }

    /// <summary>
    /// Converts a decimal value to a comma-separated string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToCommaSeperated(this decimal value)
    {
        decimal decimalsOnly = value - Math.Truncate(value);
        string[] decimalComponents = decimalsOnly.ToString().Split('.');
        string decimalText = String.Empty;
        if (decimalComponents.Length >= 2)
        {
            decimalText = "." + decimalComponents.LastOrDefault();
        }
        return Math.Truncate(value) + decimalText;
    }

    /// <summary>
    /// Converts a decimal value to a dollar value string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimalCount"></param>
    /// <returns></returns>
    public static string ToDollarValue(this decimal value, int decimalCount = 2)
    {
        return value.ToString("N" + decimalCount.ToString());
    }

    /// <summary>
    /// Converts a nullable decimal value to a percentage string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimalCount"></param>
    /// <returns></returns>
    public static string ToPercentage(this decimal? value, int decimalCount = 3)
    {
        if (value == null)
        {
            return String.Empty;
        }
        decimal convertedValue = Convert.ToDecimal(value);
        return convertedValue.ToPercentage(decimalCount);
    }

    /// <summary>
    /// Converts a nullable decimal value to a comma-separated string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToCommaSeperated(this decimal? value)
    {
        if (value == null)
        {
            return String.Empty;
        }
        decimal convertedValue = Convert.ToDecimal(value);
        return convertedValue.ToCommaSeperated();
    }

    /// <summary>
    /// Converts a nullable decimal value to a dollar value string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimalCount"></param>
    /// <returns></returns>
    public static string ToDollarValue(this decimal? value, int decimalCount = 2)
    {
        if (value == null)
        {
            return String.Empty;
        }
        decimal convertedValue = Convert.ToDecimal(value);
        return convertedValue.ToDollarValue(decimalCount);
    }

    /// <summary>
    /// Converts a decimal value to a currency string representation based on the specified culture.
    /// </summary>
    /// <remarks>
    /// <code>
    /// decimal amount = 1234.56m;
    /// string usCurrency = amount.ToCurrency("en-US"); // $1,234.56
    /// string ukCurrency = amount.ToCurrency("en-GB"); // £1,234.56
    /// </code>
    /// </remarks>
    /// <param name="value"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string ToCurrency(this decimal value, string? culture = null)
    {
        var info = culture is null
            ? CultureInfo.CurrentCulture
            : new CultureInfo(culture);
        return string.Format(info, "{0:C}", value);
    }
}