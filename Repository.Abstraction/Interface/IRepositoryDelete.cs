namespace Repository.Abstraction;

/// <summary>
/// Repository delete all items
/// </summary>
public interface IRepositoryDelete
{
    /// <summary>
    /// Delete all entities - be careful!
    /// </summary>
    /// <returns>Number of deleted items</returns>
    Task<bool> DeleteAll();
}
