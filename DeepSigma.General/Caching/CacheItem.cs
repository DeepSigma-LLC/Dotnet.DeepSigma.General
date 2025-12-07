using System.Diagnostics.CodeAnalysis;

namespace DeepSigma.General.Caching;

/// <summary>
/// Represents a cached item with its value and expiration time.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CacheItem<T> 
    where T : class
{
    /// <inheritdoc cref="CacheItem{T}"/>
    [SetsRequiredMembers]
    public CacheItem(T Value, TimeSpan timeSpan)
    {
        this.Value = Value;
        ExpirationTime = DateTime.UtcNow.Add(timeSpan);
    }

    /// <summary>
    /// The cached value.
    /// </summary>
    public required T Value { get; init; }

    /// <summary>
    /// The creation time of the cached item.
    /// </summary>
    public DateTime CreateDateTime { get; private init; } = DateTime.UtcNow;

    /// <summary>
    /// The expiration time of the cached item.
    /// </summary>
    public DateTime ExpirationTime { get; private init; }

    /// <summary>
    /// Indicates whether the cached item has expired.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpirationTime;
}
