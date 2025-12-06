using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Provides extension methods for INumber types.
/// </summary>
public static class INumberExtension
{
    /// <summary>
    /// Determines whether the specified value is closer to zero than the compareTo value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="compareTo"></param>
    /// <returns></returns>
    public static bool IsCloserToZeroThan<T>(this T value, T compareTo)
        where T : INumber<T>
    {
        return T.Abs(value) < T.Abs(compareTo);
    }
}
