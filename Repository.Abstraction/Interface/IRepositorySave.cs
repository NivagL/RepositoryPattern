namespace Repository.Abstraction;

/// <summary>
/// Repository Saves
/// </summary>
/// <typeparam name="TKey">The key of the underlying entities</typeparam>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IRepositorySave<TKey, TValue> 
{
    /// <summary>
    /// Save a value and return the status
    /// </summary>
    /// <param name="value"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    Task<bool> Save(TValue value, bool replace = true);
    //Task<Tuple<bool, ChangeEnum>> Save(TValue value, bool replace = true);

    /// <summary>
    /// Save the collection of values and return the changed statuses
    /// </summary>
    /// <param name="values"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    Task<bool> SaveAll(IEnumerable<TValue> values, bool replace = true);
    //Task<IEnumerable<Tuple<bool, ChangeEnum>>> SaveAll(
    //    IEnumerable<TValue> values, bool replace = true);

    /// <summary>
    /// Save a value and return the key and changed status
    /// </summary>
    /// <param name="value"></param>
    /// <param name="replace"></param>
    /// <returns>key and changed status</returns>
    Task<Tuple<TKey, ChangeEnum>> KeyedSave(TValue value, bool replace = true);

    /// <summary>
    /// Save the collection of values and return the keys and changed statuses
    /// </summary>
    /// <param name="values"></param>
    /// <param name="replace"></param>
    /// <returns>keys and changed statuses</returns>
    Task<IEnumerable<Tuple<TKey, ChangeEnum>>> KeyedSaveAll(
        IEnumerable<TValue> values, bool replace = true);
}
