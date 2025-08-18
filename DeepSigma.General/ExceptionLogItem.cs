
namespace DeepSigma.General
{

    /// <summary>
    /// Represents an item in \an exception log.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="friendly_message"></param>
    public class ExceptionLogItem(Exception exception, string? friendly_message = null)
    {
        /// <summary>
        /// Friendly message to be displayed in the log or to the user.
        /// </summary>
        public string? FriendlyMessage { get; set; } = friendly_message;

        /// <summary>
        /// The exception that occurred.
        /// </summary>
        public Exception Exception { get; set; } = exception;
    }
}
