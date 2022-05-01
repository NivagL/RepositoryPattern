namespace Repository.Abstraction;

/// <summary>
/// Repository Saves
/// </summary>
/// <typeparam name="TKey">The key of the underlying entities</typeparam>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IValueRepositorySave<TValue>
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
}
