namespace Repository.Abstraction;

/// <summary>
/// Repository Saves
/// </summary>
/// <typeparam name="TKey">The key of the underlying entities</typeparam>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IRepositorySave<TKey, TValue> 
{
    /// <summary>
    /// Save the collection of values and return the changed statuses
    /// </summary>
    /// <param name="values"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    Task<IEnumerable<Tuple<TKey, ChangeEnum>>> Save(IEnumerable<TValue> values
        , bool replace = true, bool trackChanges = false
        );

    /// <summary>
    /// Save a value and return the status
    /// </summary>
    /// <param name="value"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    Task<Tuple<TKey, ChangeEnum>> Save(TValue value
        , bool replace = true, bool trackChanges = false
        );
}
