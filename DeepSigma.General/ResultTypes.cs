
namespace DeepSigma.General
{
    public record Success<T>(T? Result);
    public record Error(ExceptionLogItem Exception);
    public record Errors(IEnumerable<ExceptionLogItem> Exceptions);
}
