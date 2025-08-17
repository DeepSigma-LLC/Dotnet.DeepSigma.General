using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Utilities
{
    /// <summary>
    /// Utility class for application-related operations.
    /// </summary>
    public static class AppUtilities
    {
        /// <summary>
        /// Exits the application with a non-zero exit code.
        /// </summary>
        public static void ExitApp()
        {
            Environment.Exit(1);
        }

        /// <summary>
        /// Returns the current directory of this application.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Returns assembly version of the application.
        /// </summary>
        /// <returns></returns>
        public static string GetAppVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return version?.ToString() ?? "Unknown";
        }
    }
}
