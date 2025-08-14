using System;
using DeepSigma.General.Enums;

namespace DeepSigma.General.Extensions 
{
    public static class DecimalExtensions
    {
        public static decimal Round(this decimal value,  RoundType roundType = RoundType.Normal)
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

        public static decimal DropUnnessaryPrecision(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        public static string ToPercentage(this decimal value, int decimalCount = 3)
        {
            return value.ToString("P" + decimalCount.ToString());
        }

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

        public static string ToDollarValue(this decimal value, int decimalCount = 2)
        {
            return value.ToString("N" + decimalCount.ToString());
        }

        public static string ToPercentage(this decimal? value, int decimalCount = 3)
        {
            if (value == null)
            {
                return String.Empty;
            }
            decimal convertedValue = Convert.ToDecimal(value);
            return convertedValue.ToPercentage(decimalCount);
        }

        public static string ToCommaSeperated(this decimal? value)
        {
            if(value == null)
            {
                return String.Empty;
            }
            decimal convertedValue = Convert.ToDecimal(value);
            return convertedValue.ToCommaSeperated();
        }

        public static string ToDollarValue(this decimal? value, int decimalCount = 2)
        {
            if (value == null)
            {
                return String.Empty;
            }
            decimal convertedValue = Convert.ToDecimal(value);
            return convertedValue.ToDollarValue(decimalCount);
        }
    }
}
