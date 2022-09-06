namespace Repository.Abstraction;

public interface IRepositoryFactory<TKey, TValue> : IFactory
{
    Func<IServiceProvider, IRepository<TKey, TValue>> Repository { get; set; }
    Func<IServiceProvider, IRepositoryAny<TKey, TValue>> RepositoryAny { get; set; }
    Func<IServiceProvider, IRepositoryLoad<TKey, TValue>> RepositoryLoad { get; set; }
    Func<IServiceProvider, IRepositorySave<TKey, TValue>> RepositorySave { get; set; }
    Func<IServiceProvider, IRepositoryQuery<TKey, TValue>> RepositoryQuery { get; set; }
    Func<IServiceProvider, IRepositoryDelete<TKey, TValue>> RepositoryDelete { get; set; }
}

//public interface IRepositoryFactory : IFactory
//{
//    IContextFactory ContextFactory { get; set; }
//    ICollection<IFactory> Repositories { get; set; }
//}
