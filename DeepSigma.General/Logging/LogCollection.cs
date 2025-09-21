using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Logging
{
    /// <summary>
    /// Collection of log entries.
    /// </summary>
    public class LogCollection() : IJSONSerializer<LogCollection>
    {
        /// <summary>
        /// List of log entries.
        /// </summary>
        public List<LogEntry> Entries { get; } = [];

        /// <summary>
        /// Converts the log collection to a JSON string.
        /// </summary>
        /// <returns></returns>
        public string ToJSON() => ((IJSONSerializer<LogCollection>)this).ToJSON();
    }
}
