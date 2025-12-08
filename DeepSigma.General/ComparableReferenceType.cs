
namespace DeepSigma.General;

/// <summary>
/// A reference type that implements value-based equality comparison.
/// </summary>
public abstract class ComparableReferenceType
{
    /// <summary>
    /// A unique identifier for this instance.
    /// </summary>
    public required string UniqueId { get; init; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not ComparableReferenceType other) return false;
        return UniqueId == other.UniqueId;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => UniqueId.GetHashCode();
}
