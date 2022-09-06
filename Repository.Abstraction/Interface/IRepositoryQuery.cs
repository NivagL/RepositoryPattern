using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository Queries
/// </summary>
/// <typeparam name="TKey">The key of the underlying entity</typeparam>
/// <typeparam name="TValue">The value of the underlying entity</typeparam>
public interface IRepositoryQuery<TKey, TValue>
{
    /// <summary>
    /// Load the page of values that match the expression in the order selected
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <param name="orderExpression"></param>
    /// <param name="pageFilter"></param>
    /// <param name="loadFlags"></param>
    /// <returns></returns>
    Task<PageResult<TKey, TValue>> PagedQuery(
        Expression<Func<TValue, bool>> queryExpression
        , Expression<Func<TValue, object>> orderExpression
        , PageFilter pageFilter
        );
}
