namespace Repository.Abstraction;

/// <summary>
/// Interface to see if a repository has data
/// </summary>
/// <typeparam name="TKey">The key of the underlying entity</typeparam>
/// <typeparam name="TValue">The value of the underlying entity</typeparam>
public interface IKeyedRepositoryAny<TKey, TValue>
    : IValueRepositoryAny<TValue>
{
}
