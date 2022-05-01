﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkValueRepository<TContext, TValue>
    : IValueRepositoryAny<TValue>
    where TContext : DbContext
    where TValue : class
{
    public async Task<bool> Any()
    {
        var any = await Set.AnyAsync();
        return any;
    }

    public async Task<bool> Any(Expression<Func<TValue, bool>> expression,
        LoadFlagsEnum loadFlags)
    {
        try
        {
            var queryable = Queryable(expression, loadFlags);
            return await queryable.AsNoTracking().AnyAsync();
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not check {typeof(TValue).Name}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }
}