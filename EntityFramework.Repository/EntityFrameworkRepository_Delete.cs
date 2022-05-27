using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository<TContext, TKey, TValue>
    : IRepositoryDelete<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    public async Task<TValue> Delete(TKey key)
    {
        var requestTimeout = Configuration.GetValue<TimeSpan>($"{ConfigPath}Timeout");
        using (var cancellationToken = new CancellationTokenSource(requestTimeout))
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

    public async Task<bool> DeleteAll()
    {
        try
        {
            var tableName = typeof(TValue).Name;
            var command = $"TRUNCATE TABLE {tableName}";
            //var command = $"DELETE * FROM {tableName}";

#pragma warning disable EF1000  
            int count = await Context.Database.ExecuteSqlRawAsync(command);
#pragma warning restore EF1000

            SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not delet all {typeof(TValue).Name}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }

    public async Task<int> DeleteQuery(Expression<Func<TValue, bool>> predicate)
    {
        try
        {
            var set = Set.Where(predicate).ToList();
            var count = set.Count();
            Set.RemoveRange(set);
            await Context.SaveChangesAsync();

            return count;
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not delete {typeof(TValue).Name} for expression";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }
}
