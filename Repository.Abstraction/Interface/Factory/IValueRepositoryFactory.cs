namespace Repository.Abstraction
{
    public interface IValueRepositoryFactory<TValue> : IFactory
    {
        Func<IServiceProvider, IValueRepositoryAny<TValue>> RepositoryAny { get; set; }
        Func<IServiceProvider, IValueRepositoryLoad<TValue>> RepositoryLoad { get; set; }
        Func<IServiceProvider, IValueRepositorySave<TValue>> RepositorySave { get; set; }
        Func<IServiceProvider, IValueRepositoryQuery<TValue>> RepositoryQuery { get; set; }
        Func<IServiceProvider, IValueRepositoryDelete<TValue>> RepositoryDelete { get; set; }
    }
}
