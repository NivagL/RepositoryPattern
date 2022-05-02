using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository deletes
/// </summary>
/// <typeparam name="TKey">The key of the underlying entities</typeparam>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IRepositoryDelete<TKey, TValue>
{
    /// <summary>
    /// Delete the entity with the given value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    Task<bool> Delete(TValue value);
    /// <summary>
    /// Delete all entities - be careful!
    /// </summary>
    /// <returns>Number of deleted items</returns>
    Task<bool> DeleteAll();
    /// <summary>
    /// Delete the entities that match the expression
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <returns>Number of deleted items</returns>
    Task<int> DeleteQuery(Expression<Func<TValue, bool>> queryExpression);

    /// <summary>
    /// Delete based on key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true/false if deleted or not</returns>
    Task<TValue> KeyedDelete(TKey key);
}
