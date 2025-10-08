
namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for string operations.
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// String replacement method to replace "1 = 1" in a sql query string with a new value.
    /// </summary>
    /// <param name="SQLString"></param>
    /// <param name="ReplacementString"></param>
    /// <returns></returns>
    public static string ReplaceSQL(this string SQLString, string ReplacementString)
    {
        return SQLString.Replace("1 = 1", ReplacementString);
    }

    /// <summary>
    /// Determines if string is a numeric value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string value)
    {
        return decimal.TryParse(value, out _);
    }

    /// <summary>
    /// Truncates a string to a specific max length
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <param name="truncationSuffix"></param>
    /// <returns></returns>
    public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
    {
        if (string.IsNullOrEmpty(value)) return value;

        if (value.Length <= maxLength)
        {
            maxLength = value.Length;
        }

        return value.Substring(0, maxLength) + truncationSuffix;
    }

    /// <summary>
    /// Gets file extention from end of string if string is a file name.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetExtention(this string value)
    {
        string[] strings = value.Split('.');

        if (strings.Length <= 1)
        {
            return String.Empty;
        }
        return strings[strings.Length - 1];
    }

    /// <summary>
    /// Multiplies a string by defined number. For example, "A".Multiply(2) = "AA".
    /// </summary>
    /// <param name="value"></param>
    /// <param name="MultiplicationCount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string Multiply(this string value, int MultiplicationCount)
    {
        if (MultiplicationCount < 0)
        {
            throw new ArgumentException("You my only multiply a string by a positive integer");
        }

        return String.Concat(Enumerable.Repeat(value, MultiplicationCount));
    }

}