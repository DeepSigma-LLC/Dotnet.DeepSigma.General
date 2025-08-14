using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Utilities
{
    public static class MethodUtilities
    {
        /// <summary>
        /// Gets the parameter name from a method and selected parameter index.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string? GetParameterName(System.Reflection.MethodInfo method, int index)
        {
            string? retrieved_value = null;
            if (method != null && method.GetParameters().Length > index)
            {
                retrieved_value = method.GetParameters()[index].Name;
            }
            return retrieved_value;
        }

        /// <summary>
        /// Gets parameter type from method and selected parameter index.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Type? GetParameterType(System.Reflection.MethodInfo method, int index)
        {
            Type? parameterType = null;
            if (method != null && method.GetParameters().Length > index)
            {
                parameterType = method.GetParameters()[index].ParameterType;
            }
            return parameterType;
        }

  
    }
}
