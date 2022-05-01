namespace Repository.Abstraction;

public interface IValueRepository<TValue>
    : IValueRepositoryAny<TValue>
    , IValueRepositoryDelete<TValue>
    , IValueRepositoryLoad<TValue>
    , IValueRepositoryQuery<TValue>
    , IValueRepositorySave<TValue>
{
}
