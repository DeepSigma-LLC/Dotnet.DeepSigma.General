
namespace DeepSigma.General.Logging;

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
