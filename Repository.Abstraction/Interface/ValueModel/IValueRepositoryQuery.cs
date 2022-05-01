using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository Queries
/// </summary>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IValueRepositoryQuery<TValue>
{
    Task<IEnumerable<TValue>> Query(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );

    Task<ValuePageResult<TValue>> PagedQuery(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        PageFilter pageFilter,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );

    Task<IEnumerable<TValue>> FromSql(string sql);
}
