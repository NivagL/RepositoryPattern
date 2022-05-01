namespace Repository.Abstraction;

public interface IKeyedRepository<TKey, TValue>
    : IValueRepository<TValue>
    , IKeyedRepositoryAny<TKey, TValue>
    , IKeyedRepositoryDelete<TKey, TValue>
    , IKeyedRepositoryLoad<TKey, TValue>
    , IKeyedRepositoryQuery<TKey, TValue>
    , IKeyedRepositorySave<TKey, TValue>
{
}
