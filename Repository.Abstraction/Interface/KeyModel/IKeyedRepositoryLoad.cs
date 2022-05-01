﻿using System.Linq.Expressions;

namespace Repository.Abstraction;

/// <summary>
/// Repository Loading
/// </summary>
/// <typeparam name="TKey">The key of the underlying entities</typeparam>
/// <typeparam name="TValue">The value of the underlying entities</typeparam>
public interface IKeyedRepositoryLoad<TKey, TValue>
{
    /// <summary>
    /// Load a single entity identified by the key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="loadFlags"></param>
    /// <returns></returns>
    Task<TValue> KeyedLoad(TKey key, 
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );

    /// <summary>
    /// Load a page of entities that match the expression
    /// Order is important to allow iteration over the pages
    /// </summary>
    /// <param name="pageFilter"></param>
    /// <param name="orderExpression"></param>
    /// <param name="loadFlags"></param>
    /// <returns></returns>
    Task<KeyedPageResult<TKey, TValue>> KeyedLoadAll(
        PageSelection pageSelection,
        Expression<Func<TValue, object>> orderExpression,
        SortOrderEnum sortOrder = SortOrderEnum.Ascending,
        LoadFlagsEnum loadFlags = LoadFlagsEnum.All
        );
}
