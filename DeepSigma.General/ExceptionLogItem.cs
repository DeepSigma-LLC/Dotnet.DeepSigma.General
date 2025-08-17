using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.OperatingSystem
{

    /// <summary>
    /// Represents an item in \an exception log.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="friendly_message"></param>
    public class ExceptionLogItem(Exception exception, string? friendly_message = null)
    {
        public string? FriendlyMessage { get; set; } = friendly_message;
        public Exception Exception { get; set; } = exception;
    }
}
