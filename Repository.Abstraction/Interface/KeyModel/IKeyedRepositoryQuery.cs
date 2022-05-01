using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository Queries
/// </summary>
/// <typeparam name="TKey">The key of the underlying entity</typeparam>
/// <typeparam name="TValue">The value of the underlying entity</typeparam>
public interface IKeyedRepositoryQuery<TKey, TValue>
{
    Task<IDictionary<TKey, TValue>> KeyedQuery(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );

    Task<KeyedPageResult<TKey, TValue>> KeyedPageQuery(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        PageFilter pageFilter,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );
}
