using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Abstraction;
using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository<TContext, TKey, TValue>
    : IRepositoryLoad<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    public async Task<TValue> Load(TKey key)
    {
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                TValue item = null;
                if (!Model.IsKeyTuple)
                {
                    item = await Set.FindAsync(new object[] { key }, cancellationToken.Token);
                }
                else
                {
                    var keys = new List<object>();
                    keys.AddRange(TupleUtils.TupleToEnumerable(key));
                    item = await Set.FindAsync(keys.ToArray(), cancellationToken.Token);
                }
                if (item == null)
                {
                    var userMsg = $"Repository could not load {typeof(TValue).Name} for Key {key}";
                    throw new RepositoryExceptionObjectNotFound(userMsg, "");
                }

                //LoadRelated(item, LoadFlagsEnum.All);
                return item;
            }

            catch (RepositoryExceptionObjectNotFound ex)
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                    Logger.LogWarning(ex.Message);

                throw;
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} load by key timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, RequestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} load by key error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }

    public async Task<PageResult<TKey, TValue>> LoadAll(
        PageSelection pageSelection,
        Expression<Func<TValue, object>> orderExpression,
        SortOrderEnum sortOrder)
    {
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                var data = new Dictionary<TKey, TValue>();
                var result = new List<TValue>();

                var queryable = Queryable(LoadFlagsEnum.All);
                if (!TrackQueries)
                    queryable = queryable.AsNoTracking();

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

                    foreach (var item in result)
                    {
                        data.Add(Model.GetKey(item), item);
                    }

                    var queryableCount = queryable.Count();

                    //await LoadRelated(data, loadFlags);

                    //return Tuple.Create(queryableCount, data);
                    return new PageResult<TKey, TValue>(queryableCount, pageSelection.PageNumber, pageSelection.PageSize)
                    {
                        Data = data
                    };
                }

                IQueryable<TValue> set = Set;
                if (sortOrder == SortOrderEnum.Ascending)
                {
                    set = Set
                        .OrderBy(orderExpression)
                        .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                        .Take(pageSelection.PageSize);
                }

                if (sortOrder == SortOrderEnum.Descending)
                {
                    set = Set
                        .OrderByDescending(orderExpression)
                        .Skip((pageSelection.PageNumber - 1) * pageSelection.PageSize)
                        .Take(pageSelection.PageSize);
                }

                foreach (var item in set)
                {
                    //LoadRelated(item, loadFlags);
                    data.Add(Model.GetKey(item), item);
                }

                var setCount = Set.Count();
                //return Tuple.Create(setCount, data);
                return new PageResult<TKey, TValue>(setCount, pageSelection.PageNumber, pageSelection.PageSize)
                {
                    Data = data
                };
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} load value expression timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, RequestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} load value expression error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }
}
