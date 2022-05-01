using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Interface to see if a repository has data
/// </summary>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IValueRepositoryAny<TValue>
    : IRepositoryAny
{
    /// <summary>
    /// Does the set have any items that match the expression
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="loadFlags"></param>
    /// <returns></returns>
    Task<bool> Any(Expression<Func<TValue, bool>> queryExpression,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );
}
