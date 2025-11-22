using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for enums to retrieve their description strings.
/// </summary>
public static class EnumExtension
{

    /// <summary>
    /// Checks if the enum value is defined in the enum type.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// enum Color { Red, Green, Blue }
    /// Color myColor = Color.Red;
    /// bool isDefined = myColor.IsDefined(); // returns true
    /// 
    /// Color invalidColor = (Color)100;
    /// bool isDefinedInvalid = invalidColor.IsDefined(); // returns false
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDefined<T>(this T value) where T : struct, Enum => Enum.IsDefined(typeof(T), value);


    /// <summary>
    /// Converts an enum value to its description string, using the DescriptionAttribute or DisplayAttribute if available.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// enum Status
    /// {
    ///     [Description("In Progress")]
    ///     InProgress,
    ///     [Display(Name = "Completed Successfully")]
    ///     Completed,
    ///     Failed
    /// }
    /// 
    /// Status currentStatus = Status.InProgress;
    /// string description = currentStatus.ToDescriptionString(); // returns "In Progress"
    /// </code>
    /// </remarks>
    /// <param name="value"></param>
    /// <param name="fallbackToName"></param>
    /// <returns></returns>
    public static string ToDescriptionString(this Enum value, bool fallbackToName = true)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        // For [Flags] combinations, Enum.GetName can be null
        if (name is null) return fallbackToName ? value.ToString() : string.Empty;

        var field = type.GetField(name);
        if (field is null) return fallbackToName ? name : string.Empty;

        return field.GetCustomAttribute<DescriptionAttribute>()?.Description
            ?? field.GetCustomAttribute<DisplayAttribute>()?.Name
            ?? (fallbackToName ? name : string.Empty);
    }


    /// <summary>
    /// Checks if the enum value matches any of the provided options.
    /// </summary>
    /// <remarks>
    /// <code>
    /// //Example usage:
    /// enum Color { Red, Green, Blue, Yellow }
    /// Color myColor = Color.Green;
    /// bool isPrimary = myColor.IsAnyOf(Color.Red, Color.Green, Color.Blue); // returns true
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool IsAnyOf<T>(this T value, params T[] options) where T : struct, Enum => options.Contains(value);

    /// <summary>
    /// Converts an enum value to its display string using the DisplayAttribute if available.
    /// </summary>
    /// <remarks>
    /// <code>
    /// //Example usage:
    /// enum Status
    /// {
    ///     [Display(Name = "In Progress")]
    ///     InProgress,
    ///     [Display(Name = "Completed Successfully")]
    ///     Completed,
    ///     Failed
    /// }
    /// Status currentStatus = Status.InProgress;
    /// string displayString = currentStatus.ToDisplayString(); // returns "In Progress"
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToDisplayString<T>(this T value) where T : struct, Enum
    {
        var field = typeof(T).GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DisplayAttribute>();
        return attr?.Name ?? value.ToString();
    }

    /// <summary>
    /// Converts an enum value to its integer representation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToInt<T>(this T value) where T : struct, Enum => Convert.ToInt32(value);

    /// <summary>
    /// Converts an enum value to its long representation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long ToLong<T>(this T value) where T : struct, Enum => Convert.ToInt64(value);


    /// <summary>
    /// Converts an integer to its corresponding enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T EnumFromInt<T>(int value) where T : struct, Enum => (T)Enum.ToObject(typeof(T), value);

    /// <summary>
    /// Gets a dictionary mapping enum values to their string representations.
    /// Helpful for populating dropdowns or selection lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Dictionary<T, string> ToDictionary<T>() where T : struct, Enum =>
    Enum.GetValues(typeof(T))
        .Cast<T>()
        .ToDictionary(v => v, v => v.ToString());

    /// <summary>
    /// Gets all enum names of the specified enum type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<string> GetNames<T>() where T : struct, Enum => Enum.GetNames(typeof(T));

    /// <summary>
    /// Gets all enum values of the specified enum type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetAllEnums<T>() where T : struct, Enum => Enum.GetValues(typeof(T)).Cast<T>();


}
