using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Logging
{
    /// <summary>
    /// Custom file logger option properties.
    /// </summary>
    public class FileLoggerOptions
    {
        /// <summary>
        /// Target file name.
        /// </summary>
        public virtual required string FileName { get; set; } = "{Username}-Log-{Date}.json";
        /// <summary>
        /// Directory path storing the log files.
        /// </summary>
        public virtual required string FolderPath { get; set; }
    }
}
