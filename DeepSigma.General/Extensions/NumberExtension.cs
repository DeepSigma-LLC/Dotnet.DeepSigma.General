
using System.Numerics;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Provides extension methods for numeric types, including absolute value calculation.
/// </summary>
public static class NumberExtension
{
    /// <summary>
    /// Returns the absolute value of a decimal number.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T AbsoluteValue<T>(this T value) where T : struct, INumber<T>
    {
        return T.Abs(value);
    }

    /// <inheritdoc cref="AbsoluteValue{T}(T)"/>
    public static T? AbsoluteValue<T>(this T? value) where T : struct, INumber<T>
    {
        return value.HasValue ? value.Value : null;
    }
}
