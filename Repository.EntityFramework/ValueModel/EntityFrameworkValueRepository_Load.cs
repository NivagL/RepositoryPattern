using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkValueRepository<TContext, TValue>
    : IValueRepositoryLoad<TValue>
    where TContext : DbContext
    where TValue : class
{
    public async Task<ValuePageResult<TValue>> LoadAll(
        PageSelection pageSelection,
        Expression<Func<TValue, object>> orderExpression,
        SortOrderEnum sortOrder,
        LoadFlagsEnum loadFlags)
    {
        try
        {
            var data = new List<TValue>();

            var result = new List<TValue>();
            var queryable = Queryable(loadFlags);
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
                    data.Add(item);
                }

                var queryableCount = queryable.Count();
                
                //await LoadRelated(data, loadFlags);

                //return Tuple.Create(queryableCount, data);
                return new ValuePageResult<TValue>(queryableCount, pageSelection.PageNumber, pageSelection.PageSize)
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
                data.Add(item);
            }

            var setCount = Set.Count();
            //return Tuple.Create(setCount, data);
            return new ValuePageResult<TValue>(setCount, pageSelection.PageNumber, pageSelection.PageSize)
            {
                Data = data
            };
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not load {typeof(TValue).Name}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }
}
