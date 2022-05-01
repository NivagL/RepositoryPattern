using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository deletes
/// </summary>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IValueRepositoryDelete<TValue>
    : IRepositoryDelete
{
    /// <summary>
    /// Delete the entity with the given value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    Task<bool> Delete(TValue value);
    /// <summary>
    /// Delete the entities that match the expression
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <returns>Number of deleted items</returns>
    Task<int> DeleteQuery(Expression<Func<TValue, bool>> queryExpression);
}
