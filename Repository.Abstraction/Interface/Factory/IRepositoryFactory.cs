namespace Repository.Abstraction;

public interface IRepositoryFactory : IFactory
{
    IContextFactory ContextFactory { get; set; }
    ICollection<IFactory> KeyedRepositories { get; set; }
    ICollection<IFactory> ValueRepositories { get; set; }
}
