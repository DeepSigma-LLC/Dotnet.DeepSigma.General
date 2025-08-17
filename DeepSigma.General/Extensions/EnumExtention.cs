using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Extensions
{
    /// <summary>
    /// Extension methods for enums to retrieve their description strings.
    /// </summary>
    public static class EnumExtention
    {
        /// <summary>
        /// Converts an enum value to its description string, using the DescriptionAttribute or DisplayAttribute if available.
        /// </summary>
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
    }
}
