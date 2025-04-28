namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// Defines a contract for caching operations, including storing, retrieving, and removing cached items.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Stores a value in the cache with the specified key and optional expiration time.
    /// </summary>
    /// <typeparam name="T">The type of the value to be cached.</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <param name="value">The value to store in the cache.</param>
    /// <param name="expiration">The optional expiration time for the cached item. If null, the item does not expire.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration);

    /// <summary>
    /// Retrieves a value from the cache by its key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the cached value, 
    /// or null if the key does not exist in the cache.
    /// </returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Removes a value from the cache by its key.
    /// </summary>
    /// <param name="key">The unique key identifying the cached item to remove.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is true if the item was successfully removed; 
    /// otherwise, false.
    /// </returns>
    Task<bool> RemoveAsync(string key);
}