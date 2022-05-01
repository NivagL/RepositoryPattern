using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkKeyedRepository<TContext, TKey, TValue>
    : EntityFrameworkValueRepository<TContext, TValue>
    , IKeyedRepositoryQuery<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    public async Task<IDictionary<TKey, TValue>> KeyedQuery(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        LoadFlagsEnum loadFlags)
    {
        try
        {
            var data = new Dictionary<TKey, TValue>();

            var queryable = Queryable(queryExpression, loadFlags);
            if (queryable != null)
            {
                var result = TrackQueries ? await queryable.ToListAsync() : await queryable.AsNoTracking().ToListAsync();
                if (result != null)
                {
                    if (KeyedModel.GetKey != null)
                    {
                        foreach (var item in result)
                        {
                            data.Add(KeyedModel.GetKey(item), item);
                        }
                    }
                    await LoadRelated(data, loadFlags);
                    return data;
                }
                else
                    Logger.LogWarning($"Null result for queryable in {typeof(TValue)} ModelRepository");
            }
            else
                Logger.LogWarning($"Null queryable in {typeof(TValue)} ModelRepository");

            var result1 = TrackQueries ? await Set.Where(queryExpression).ToListAsync() : await Set.Where(queryExpression).AsNoTracking().ToListAsync();
            if (result1 != null)
            {
                if (KeyedModel.GetKey != null)
                {
                    foreach (var item in result1)
                    {
                        //LoadRelated(item, loadFlags);
                        data.Add(KeyedModel.GetKey(item), item);
                    }
                }
            }
            else
                Logger.LogWarning($"Null result for expression in {typeof(TValue)} ModelRepository");

            return data;
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not load {typeof(TValue).Name} for key expression";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }

    /// <summary>
    /// Support for pagable queries
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <param name="sortOrder"></param>
    /// <param name="loadFlags"></param>
    /// <param name="pageSelection"></param>
    /// <returns></returns>
    public async Task<KeyedPageResult<TKey, TValue>> KeyedPageQuery(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        PageFilter pageFilter, 
        LoadFlagsEnum loadFlags
        )
    {
        try
        {
            var sortOrder = pageFilter.SortOrder;
            var pageSelection = pageFilter.PageSelection;

            var data = new Dictionary<TKey, TValue>();

            List<TValue> result = new List<TValue>();
            var queryable = Queryable(queryExpression, loadFlags);
            if (queryable != null)
            {
                if (TrackQueries)
                {
                    if (sortOrder == SortOrderEnum.Ascending)
                    {
                        result = await queryable
                            .OrderBy(orderExpression)
                            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                            .Take(pageSelection.PageSize)
                            .ToListAsync();
                    }

                    if (sortOrder == SortOrderEnum.Descending)
                    {
                        result = await queryable
                            .OrderByDescending(orderExpression)
                            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                            .Take(pageSelection.PageSize)
                            .ToListAsync();
                    }
                }
                if (!TrackQueries)
                {
                    if (sortOrder == SortOrderEnum.Ascending)
                    {
                        result = await queryable.AsNoTracking()
                            .OrderBy(orderExpression)
                            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                            .Take(pageSelection.PageSize)
                            .ToListAsync();
                    }

                    if (sortOrder == SortOrderEnum.Descending)
                    {
                        result = await queryable.AsNoTracking()
                            .OrderByDescending(orderExpression)
                            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                            .Take(pageSelection.PageSize)
                            .ToListAsync();
                    }
                }

                if (result != null)
                {
                    if (KeyedModel.GetKey != null)
                    {
                        foreach (var item in result)
                        {
                            data.Add(KeyedModel.GetKey(item), item);
                        }
                    }

                    var queryableCount = queryable.Count();
                    //was this ever needed? Queryable handles it
                    //await LoadRelated(data, loadFlags);

                    //return Tuple.Create(queryableCount, data);
                    return new KeyedPageResult<TKey, TValue>(queryableCount, pageSelection.PageNumber, pageSelection.PageSize)
                    {
                        Data = data
                    };
                }
                else
                    Logger.LogWarning($"Null result for queryable in {typeof(TValue)} ModelRepository");
            }
            else
                Logger.LogWarning($"Null queryable in {typeof(TValue)} ModelRepository");

            List<TValue> result1 = new List<TValue>();
            if (TrackQueries)
            {
                if (sortOrder == SortOrderEnum.Ascending)
                {
                    result1 = await Set.Where(queryExpression)
                        .OrderBy(orderExpression)
                        .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                        .Take(pageSelection.PageSize)
                        .ToListAsync();
                }

                if (sortOrder == SortOrderEnum.Descending)
                {
                    result1 = await Set.Where(queryExpression)
                        .OrderByDescending(orderExpression)
                        .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                        .Take(pageSelection.PageSize)
                        .ToListAsync();
                }
            }

            if (!TrackQueries)
            {
                if (sortOrder == SortOrderEnum.Ascending)
                {
                    result1 = await Set.Where(queryExpression)
                        .OrderBy(orderExpression)
                        .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                        .Take(pageSelection.PageSize)
                        .AsNoTracking()
                        .ToListAsync();
                }

                if (sortOrder == SortOrderEnum.Descending)
                {
                    result1 = await Set.Where(queryExpression)
                        .OrderByDescending(orderExpression)
                        .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                        .Take(pageSelection.PageSize)
                        .AsNoTracking()
                        .ToListAsync();
                }
            }

            if (result1 != null)
            {
                if (KeyedModel.GetKey != null)
                { 
                    foreach (var item in result1)
                    {
                        //LoadRelated(item, loadFlags);
                        data.Add(KeyedModel.GetKey(item), item);
                    }
                }
            }
            else
                Logger.LogWarning($"Null result for expression in {typeof(TValue)} ModelRepository");

            var setCount = Set.Where(queryExpression).Count();

            //return Tuple.Create(setCount, data);
            return new KeyedPageResult<TKey, TValue>(setCount, pageSelection.PageNumber, pageSelection.PageSize)
            {
                Data = data
            };
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not query {typeof(TValue).Name} for expression";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }
}
