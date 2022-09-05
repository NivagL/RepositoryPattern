using System.ComponentModel;
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
    /// Delete all entities - be careful!
    /// </summary>
    /// <returns>Number of deleted items</returns>
    Task<int> DeleteAll(int chunks = 1000);

    /// <summary>
    /// Delete based on key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true/false if deleted or not</returns>
    Task<TValue> Delete(TKey key);

    /// <summary>
    /// Delete the entities that match the expression
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <returns>Number of deleted items</returns>
    Task<int> DeleteQuery(Expression<Func<TValue, bool>> queryExpression);
}
