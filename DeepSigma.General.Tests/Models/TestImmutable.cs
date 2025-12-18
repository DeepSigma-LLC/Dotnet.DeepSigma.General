using System.Diagnostics.CodeAnalysis;

namespace DeepSigma.General.Tests.Models;

internal class TestImmutable
{
    [SetsRequiredMembers]
    internal TestImmutable(int value)
    {
        Value = value;
    }

    internal AbsoluteValueImmutable<int>? _AbsoluteValueImmutable { get; init; }

    internal required int Value
    {
        get => _AbsoluteValueImmutable?.Value ?? 0;
        init => _AbsoluteValueImmutable = new() { Value = value };
    }
}
