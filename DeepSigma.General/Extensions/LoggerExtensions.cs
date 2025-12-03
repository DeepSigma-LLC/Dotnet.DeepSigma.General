
using Microsoft.Extensions.Logging;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for ILogger.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Tries to log an error message with an optional exception.
    /// Note: If the logger is null (or the error is null), no action is taken.
    /// </summary>
    /// <param name="Logger"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    public static void TryToLogErrorOnlyIfException(this ILogger? Logger, Exception? exception, string? message)
    {
        if (exception == null) return;
        Logger.TryToLogError(message, exception);
    }

    /// <summary>
    /// Tries to log an error message with an optional exception.
    /// Note: If the logger is null, no action is taken.
    /// </summary>
    /// <param name="Logger"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public static void TryToLogError(this ILogger? Logger, string? message, Exception? exception)
    {
        Logger?.LogError(exception, message);
    }


    /// <summary>
    /// Tries to log an information message with an optional exception.
    /// Note: If the logger is null, no action is taken.
    /// </summary>
    /// <param name="Logger"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public static void TryToLogInformation(this ILogger? Logger, string? message, Exception? exception)
    {
        Logger?.LogInformation(message);
    }

    /// <summary>
    /// Tries to log a warning message with an optional exception.
    /// Note: If the logger is null, no action is taken.
    /// </summary>
    /// <param name="Logger"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public static void TryToLogWarning(this ILogger? Logger, string message, Exception? exception)
    {
        Logger?.LogWarning(exception, message);
    }
}
