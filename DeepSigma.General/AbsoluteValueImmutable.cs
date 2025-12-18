using System.Numerics;

namespace DeepSigma.General;

/// <summary>
/// A generic quantity class that stores the absolute value of a quantity.
/// </summary>
/// <remarks>
/// Sealed to prevent inheritance. Favor composition over inheritance.
/// </remarks>
public sealed class AbsoluteValueImmutable<T> where T : INumber<T>
{
    /// <summary>
    /// The value stored as an absolute value.
    /// </summary>
    public required T Value
    {
        get => field;
        init => field = T.Abs(value);
    }
}
