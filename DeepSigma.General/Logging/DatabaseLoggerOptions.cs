using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Logging
{
    /// <summary>
    /// Custom database logger options.
    /// </summary>
    public class DatabaseLoggerOptions
    {
        /// <summary>
        /// Database connection string.
        /// </summary>
        public virtual required string ConnectionString { get; set; }

        /// <summary>
        /// Delegate method containing the logging logic.
        /// </summary>
        public Action<LogCollection>? LogToDatabase { get; set; }
    }
}
