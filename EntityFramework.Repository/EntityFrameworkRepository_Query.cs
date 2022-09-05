using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository<TContext, TKey, TValue>
    : IRepositoryQuery<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    //public async Task<IDictionary<TKey, TValue>> KeyedQuery(
    //    Expression<Func<TValue, bool>> queryExpression,
    //    Expression<Func<TValue, object>> orderExpression,
    //    LoadFlagsEnum loadFlags)
    //{
    //    try
    //    {
    //        var data = new Dictionary<TKey, TValue>();

    //        var queryable = Queryable(queryExpression, loadFlags);
    //        if (queryable != null)
    //        {
    //            var result = TrackQueries ? await queryable.ToListAsync() : await queryable.AsNoTracking().ToListAsync();
    //            if (result != null)
    //            {
    //                if (KeyedModel.GetKey != null)
    //                {
    //                    foreach (var item in result)
    //                    {
    //                        data.Add(KeyedModel.GetKey(item), item);
    //                    }
    //                }
    //                await LoadRelated(data, loadFlags);
    //                return data;
    //            }
    //            else
    //                Logger.LogWarning($"Null result for queryable in {typeof(TValue)} ModelRepository");
    //        }
    //        else
    //            Logger.LogWarning($"Null queryable in {typeof(TValue)} ModelRepository");

    //        var result1 = TrackQueries ? await Set.Where(queryExpression).ToListAsync() : await Set.Where(queryExpression).AsNoTracking().ToListAsync();
    //        if (result1 != null)
    //        {
    //            if (KeyedModel.GetKey != null)
    //            {
    //                foreach (var item in result1)
    //                {
    //                    //LoadRelated(item, loadFlags);
    //                    data.Add(KeyedModel.GetKey(item), item);
    //                }
    //            }
    //        }
    //        else
    //            Logger.LogWarning($"Null result for expression in {typeof(TValue)} ModelRepository");

    //        return data;
    //    }
    //    catch (Exception ex)
    //    {
    //        var userMsg = $"Repository could not load {typeof(TValue).Name} for key expression";
    //        var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
    //        Logger.LogError($"{userMsg} - {msg}");

    //        throw new RepositoryException(userMsg, msg);
    //    }
    //}

    /// <summary>
    /// Support for pagable queries
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <param name="sortOrder"></param>
    /// <param name="loadFlags"></param>
    /// <param name="pageSelection"></param>
    /// <returns></returns>
    public async Task<PageResult<TKey, TValue>> Query(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        PageFilter pageFilter)
    {
        try
        {
            var queryable = Queryable(queryExpression, LoadFlagsEnum.All);
            if (queryable != null && !TrackQueries)
                queryable = queryable.AsNoTracking();

            var sortOrder = pageFilter.SortOrder;
            var pageSelection = pageFilter.PageSelection;

            var data = new Dictionary<TKey, TValue>();
            List<TValue> result = new List<TValue>();

            if (queryable != null)
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

                if (result != null)
                {
                    if (Model.GetKey != null)
                    {
                        foreach (var item in result)
                        {
                            data.Add(Model.GetKey(item), item);
                        }
                    }

                    var queryableCount = queryable.Count();
                    return new PageResult<TKey, TValue>(queryableCount, pageSelection.PageNumber, pageSelection.PageSize)
                    {
                        Data = data
                    };
                }
                else
                    Logger.LogWarning($"Null result for queryable in {typeof(TValue)} ModelRepository");
            }
            else
                Logger.LogWarning($"Null queryable in {typeof(TValue)} ModelRepository");

            throw new Exception("shouldn get here...");
            //List<TValue> result1 = new List<TValue>();
            //if (TrackQueries)
            //{
            //    if (sortOrder == SortOrderEnum.Ascending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderBy(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .ToListAsync();
            //    }

            //    if (sortOrder == SortOrderEnum.Descending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderByDescending(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .ToListAsync();
            //    }
            //}

            //if (!TrackQueries)
            //{
            //    if (sortOrder == SortOrderEnum.Ascending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderBy(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .AsNoTracking()
            //            .ToListAsync();
            //    }

            //    if (sortOrder == SortOrderEnum.Descending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderByDescending(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .AsNoTracking()
            //            .ToListAsync();
            //    }
            //}

            //if (result1 != null)
            //{
            //    if (KeyedModel.GetKey != null)
            //    { 
            //        foreach (var item in result1)
            //        {
            //            //LoadRelated(item, loadFlags);
            //            data.Add(KeyedModel.GetKey(item), item);
            //        }
            //    }
            //}
            //else
            //    Logger.LogWarning($"Null result for expression in {typeof(TValue)} ModelRepository");

            //var setCount = Set.Where(queryExpression).Count();

            ////return Tuple.Create(setCount, data);
            //return new KeyedPageResult<TKey, TValue>(setCount, pageSelection.PageNumber, pageSelection.PageSize)
            //{
            //    Data = data
            //};
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not query {typeof(TValue).Name} for expression";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="expression"></param>
    ///// <param name="orderExpression"></param>
    ///// <param name="loadFlags"></param>
    ///// <returns></returns>
    ///// <exception cref="RepositoryException"></exception>
    //public async Task<IEnumerable<TValue>> Query(
    //    Expression<Func<TValue, bool>> expression,
    //    Expression<Func<TValue, object>> orderExpression,
    //    LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
    //{
    //    try
    //    {
    //        var data = new List<TValue>();

    //        var queryable = Queryable(expression, loadFlags);
    //        if (queryable != null)
    //        {
    //            var result = TrackQueries ? await queryable.ToListAsync() : await queryable.AsNoTracking().ToListAsync();
    //            if (result != null)
    //            {
    //                foreach (var item in result)
    //                {
    //                    data.Add(item);
    //                }
    //                await LoadRelated(data, loadFlags);
    //                return data;
    //            }
    //            else
    //                Logger.LogWarning($"Null result for queryable in {typeof(TValue)} ModelRepository");
    //        }
    //        else
    //            Logger.LogWarning($"Null queryable in {typeof(TValue)} ModelRepository");

    //        var result1 = TrackQueries ? await Set.Where(expression).ToListAsync() : await Set.Where(expression).AsNoTracking().ToListAsync();
    //        if (result1 != null)
    //        {
    //            foreach (var item in result1)
    //            {
    //                //LoadRelated(item, loadFlags);
    //                data.Add(item);
    //            }
    //        }
    //        else
    //            Logger.LogWarning($"Null result for expression in {typeof(TValue)} ModelRepository");

    //        return data;
    //    }
    //    catch (Exception ex)
    //    {
    //        var userMsg = $"Repository could not query {typeof(TValue).Name} for expression";
    //        var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
    //        Logger.LogError($"{userMsg} - {msg}");

    //        throw new RepositoryException(userMsg, msg);
    //    }
    //}

    //public async Task<IEnumerable<TValue>> FromSql(string sql)
    //{
    //    var data = new List<TValue>();

    //    var queryable = await Set.FromSqlRaw(sql).AsNoTracking().ToListAsync();
    //    if (queryable != null)
    //    {
    //        foreach (var item in queryable)
    //        {
    //            data.Add(item);
    //        }
    //    }

    //    return data;
    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryExpression"></param>
    /// <param name="orderExpression"></param>
    /// <param name="pageFilter"></param>
    /// <param name="loadFlags"></param>
    /// <returns></returns>
    /// <exception cref="RepositoryException"></exception>
    public async Task<PageResult<TKey, TValue>> PagedQuery(
        Expression<Func<TValue, bool>> queryExpression,
        Expression<Func<TValue, object>> orderExpression,
        PageFilter pageFilter)
    {
        try
        {
            var sortOrder = pageFilter.SortOrder;
            var pageSelection = pageFilter.PageSelection;

            var data = new Dictionary<TKey, TValue>();

            List<TValue> result = new List<TValue>();
            var queryable = Queryable(queryExpression, LoadFlagsEnum.All);
            if (queryable != null)
            {
                if (!TrackQueries)
                    queryable = queryable.AsNoTracking();

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

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        data.Add(Model.GetKey(item), item);
                    }

                    var queryableCount = queryable.Count();
                    return new PageResult<TKey, TValue>(queryableCount, pageSelection.PageNumber, pageSelection.PageSize)
                    {
                        Data = data
                    };
                }
                else
                    Logger.LogWarning($"Null result for queryable in {typeof(TValue)} ModelRepository");
            }
            else
                Logger.LogWarning($"Null queryable in {typeof(TValue)} ModelRepository");

            throw new Exception("Should not get here...");
            //List<TValue> result1 = new List<TValue>();
            //if (TrackQueries)
            //{
            //    if (sortOrder == SortOrderEnum.Ascending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderBy(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .ToListAsync();
            //    }

            //    if (sortOrder == SortOrderEnum.Descending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderByDescending(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .ToListAsync();
            //    }
            //}

            //if (!TrackQueries)
            //{
            //    if (sortOrder == SortOrderEnum.Ascending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderBy(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .AsNoTracking()
            //            .ToListAsync();
            //    }

            //    if (sortOrder == SortOrderEnum.Descending)
            //    {
            //        result1 = await Set.Where(queryExpression)
            //            .OrderByDescending(orderExpression)
            //            .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
            //            .Take(pageSelection.PageSize)
            //            .AsNoTracking()
            //            .ToListAsync();
            //    }
            //}

            //if (result1 != null)
            //{
            //    foreach (var item in result1)
            //    {
            //        //LoadRelated(item, loadFlags);
            //        data.Add(item);
            //    }
            //}
            //else
            //    Logger.LogWarning($"Null result for expression in {typeof(TValue)} ModelRepository");

            //var setCount = Set.Where(queryExpression).Count();

            ////return Tuple.Create(setCount, data);
            //return new ValuePageResult<TValue>(setCount, pageSelection.PageNumber, pageSelection.PageSize)
            //{
            //    Data = data
            //};
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
