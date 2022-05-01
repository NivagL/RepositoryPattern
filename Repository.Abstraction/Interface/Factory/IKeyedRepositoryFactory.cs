using Microsoft.Extensions.DependencyInjection;

namespace Repository.Abstraction;

public interface IKeyedRepositoryFactory<TKey, TValue> : IFactory
{
    Func<IServiceProvider, IKeyedRepositoryAny<TKey, TValue>> RepositoryAny { get; set; }
    Func<IServiceProvider, IKeyedRepositoryLoad<TKey, TValue>> RepositoryLoad { get; set; }
    Func<IServiceProvider, IKeyedRepositorySave<TKey, TValue>> RepositorySave { get; set; }
    Func<IServiceProvider, IValueRepositoryQuery<TValue>> RepositoryQuery { get; set; }
    Func<IServiceProvider, IKeyedRepositoryDelete<TKey, TValue>> RepositoryDelete { get; set; }
}
