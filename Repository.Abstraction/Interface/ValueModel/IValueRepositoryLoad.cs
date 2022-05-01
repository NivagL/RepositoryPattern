using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository Loading
/// </summary>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IValueRepositoryLoad<TValue>
{
    /// <summary>
    /// Load a page of entities that match the expression
    /// Order is important to allow iteration over the pages
    /// </summary>
    /// <param name="pageFilter"></param>
    /// <param name="orderExpression"></param>
    /// <param name="loadFlags"></param>
    /// <returns></returns>
    Task<ValuePageResult<TValue>> LoadAll(
        PageSelection pageSelection,
        Expression<Func<TValue, object>> orderExpression,
        SortOrderEnum sortOrder = SortOrderEnum.Ascending,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );
}
