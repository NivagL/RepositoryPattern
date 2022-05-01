namespace Repository.Abstraction;

/// <summary>
/// Interface to see if a repository has data
/// </summary>
public interface IRepositoryAny
{
    /// <summary>
    /// Does the set have any items
    /// </summary>
    /// <returns>true/false</returns>
    Task<bool> Any();
}
