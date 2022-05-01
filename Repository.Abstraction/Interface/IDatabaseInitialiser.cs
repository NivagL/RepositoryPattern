namespace Repository.Abstraction;

public interface IDatabaseInitialiser
{
    Task<bool> InitialiseDatabase(IServiceProvider serviceProvider);
}
