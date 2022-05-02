namespace Repository.Abstraction;

public interface IRepository<TKey, TValue>
    : IRepositoryAny<TKey, TValue>
    , IRepositoryDelete<TKey, TValue>
    , IRepositoryLoad<TKey, TValue>
    , IRepositoryQuery<TKey, TValue>
    , IRepositorySave<TKey, TValue>
{
}
