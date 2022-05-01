using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkValueRepository<TContext, TValue>
    : IValueRepositoryDelete<TValue>
    where TContext : DbContext
    where TValue : class
{
    public async Task<bool> Delete(TValue value)
    {
        try
        {
            var existing = await Set.FindAsync(value);
            if (existing != null)
            {
                RemoveRelated(existing);
                Set.Remove(existing);
                await Context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not delete {typeof(TValue).Name}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
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
