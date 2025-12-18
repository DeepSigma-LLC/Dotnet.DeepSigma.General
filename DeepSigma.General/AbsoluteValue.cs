using System.Numerics;

namespace DeepSigma.General;

/// <summary>
/// A generic quantity class that stores the absolute value of a quantity.
/// </summary>
/// <remarks>
/// Sealed to prevent inheritance. Favor composition over inheritance.
/// </remarks>
public sealed class AbsoluteValue<T>() where T : INumber<T>
{
    /// <inheritdoc cref="AbsoluteValue{T}"/>
    public AbsoluteValue(T value) : this()
    {
        this.Value = value;
    }

    /// <summary>
    /// The value stored as an absolute value.
    /// </summary>
    public T Value
    {
        get => field;
        set => field = T.Abs(value);
    } = T.Zero;
}
