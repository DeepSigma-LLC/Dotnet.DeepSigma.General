using DeepSigma.General.Extensions;

namespace DeepSigma.General.Caching;

/// <summary>
/// A simple local cache implementation with expiration support.
/// </summary>
/// <typeparam name="TIndex"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class LocalCache<TIndex, TValue> 
    where TIndex : notnull
    where TValue : class
{
    /// <summary>
    /// The internal cache storage.
    /// </summary>
    private readonly Dictionary<TIndex, CacheItem<TValue>> cache = [];

    /// <summary>
    /// The time to live for each cached item.
    /// </summary>
    private TimeSpan TimeToLive { get; init; }

    /// <inheritdoc cref="LocalCache{TIndex, TValue}"/>
    public LocalCache(TimeSpan time_to_live)
    {
        TimeToLive = time_to_live;
    }

    /// <summary>
    /// Adds a new item to the cache.
    /// If the item already exists, it will not be updated.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="item_value"></param>
    public void AddWithNoUpdate(TIndex item_index, TValue item_value)
    {
        if (cache.ContainsKey(item_index))
        {
            if(cache[item_index].IsExpired == false) return;
            cache.Remove(item_index);
        }
        cache[item_index] = new(item_value, TimeToLive);
    }

    /// <summary>
    /// Adds a new item to the cache.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="item_value"></param>
    public void AddWithUpdate(TIndex item_index, TValue item_value)
    {
        cache.TryAdd(item_index, new CacheItem<TValue>(item_value, TimeToLive));
    }

    /// <summary>
    /// Tries to get a value from the cache.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public TValue? TryGet(TIndex index)
    {
        cache.TryGetValue(index, out CacheItem<TValue>? value);
        if(value is null) return null;
        if(value.IsExpired)
        {
            cache.Remove(index);
            return null;
        }
        return value.Value;
    }

    /// <summary>
    /// Cleans up expired items from the cache.
    /// </summary>
    public void RemoveAllExpiredItems()
    {
        List<TIndex> keys_to_remove = cache
            .Where(kvp => kvp.Value.IsExpired)
            .Select(kvp => kvp.Key)
            .ToList();
        keys_to_remove.ForEach(key => cache.Remove(key));
    }

    /// <summary>
    /// Removes a specific item from the cache.
    /// </summary>
    /// <param name="index"></param>
    public void Remove(TIndex index)
    {
        cache.Remove(index);
    }

    /// <summary>
    /// Clears all exchange rates from the cache.
    /// </summary>
    public void Clear()
    {
        cache.Clear();
    }
}
