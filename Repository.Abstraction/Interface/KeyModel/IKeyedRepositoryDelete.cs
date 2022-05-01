namespace Repository.Abstraction;

/// <summary>
/// Repository deletes
/// </summary>
/// <typeparam name="TKey">The key of the underlying entities</typeparam>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IKeyedRepositoryDelete<TKey, TValue>
    : IValueRepositoryDelete<TValue>
{
    /// <summary>
    /// Delete based on key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true/false if deleted or not</returns>
    Task<TValue> KeyedDelete(TKey key);
}
