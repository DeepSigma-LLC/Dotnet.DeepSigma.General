

namespace DeepSigma.General.Extensions;

/// <summary>
/// Provides extension methods for the Math class to support decimal types.
/// </summary>
public static class MathExtension
{
    extension(Math)
    {
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

    }
}
