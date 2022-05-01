using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkKeyedRepository<TContext, TKey, TValue>
    : EntityFrameworkValueRepository<TContext, TValue>
    , IKeyedRepositoryDelete<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    public async Task<TValue> KeyedDelete(TKey key)
    {
        try
        {
            TValue existing = null;
            if (!KeyedModel.IsKeyTuple)
            {
                existing = await Set.FindAsync(key);
            }
            else
            {
                var keys = new List<object>();
                keys.AddRange(TupleUtils.TupleToEnumerable(key));
                existing = await Set.FindAsync(keys.ToArray());
            }
            if (existing != null)
            {
                RemoveRelated(existing);
                Set.Remove(existing);
                await Context.SaveChangesAsync();
            }

            return existing;
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not delete {typeof(TValue).Name} for key {key}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError($"{userMsg} - {msg}");
            throw new RepositoryException(userMsg, msg);
        }
    }
}
