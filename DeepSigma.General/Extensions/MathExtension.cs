

namespace DeepSigma.General.Extensions;

/// <summary>
/// Provides extension methods for the Math class to support decimal types.
/// </summary>
public static class MathExtension
{
    extension(Math)
    {
        /// <summary>
        /// Calculates the power of a decimal value raised to another decimal value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>

        public static decimal Pow(decimal x, decimal y) => Math.Pow(x.ToDouble(), y.ToDouble()).ToDecimal();

        /// <summary>
        /// Calculates the power of a nullable decimal value raised to another nullable decimal value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal? Pow(decimal? x, decimal? y) => (x.HasValue && y.HasValue) ? Pow(x.Value, y.Value) : null;

        /// <summary>
        /// Calculates the square root of a decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Sqrt(decimal value) => Math.Sqrt(value.ToDouble()).ToDecimal();

        /// <summary>
        /// Calculates the square root of a nullable decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? Sqrt(decimal? value) => value.HasValue ? Sqrt(value.Value) : null;

        /// <summary>
        /// Calculates the exponential of a decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Exp(decimal value) => Math.Exp(value.ToDouble()).ToDecimal();

        /// <summary>
        /// Calculates the exponential of a nullable decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? Exp(decimal? value) => value.HasValue ? Exp(value.Value) : null;

        /// <summary>
        /// Calculates the natural logarithm of a decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Log(decimal value) => Math.Log(value.ToDouble()).ToDecimal();

        /// <summary>
        /// Calculates the natural logarithm of a nullable decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? Log(decimal? value) => value.HasValue ? Log(value.Value) : null;

        /// <summary>
        /// Calculates the base-10 logarithm of a decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Log10(decimal value) => Math.Log10(value.ToDouble()).ToDecimal();

        /// <summary>
        /// Calculates the base-10 logarithm of a nullable decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? Log10(decimal? value) => value.HasValue ? Log10(value.Value) : null;

        /// <summary>
        /// Returns the smaller of two decimal values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal Min(decimal a, decimal b) => a < b ? a : b;

        /// <summary>
        /// Returns the smaller of two nullable decimal values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal? Min(decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
            {
                return Min(a.Value, b.Value);
            }
            else if (a.HasValue)
            {
                return a.Value;
            }
            else if (b.HasValue)
            {
                return b.Value;
            }
            return null;
        }

        /// <summary>
        /// Returns the larger of two decimal values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal Max(decimal a, decimal b) => a > b ? a : b;

        /// <summary>
        /// Returns the larger of two nullable decimal values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal? Max(decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
            {
                return Max(a.Value, b.Value);
            }
            else if (a.HasValue)
            {
                return a.Value;
            }
            else if (b.HasValue)
            {
                return b.Value;
            }
            return null;
        }

    }
}
