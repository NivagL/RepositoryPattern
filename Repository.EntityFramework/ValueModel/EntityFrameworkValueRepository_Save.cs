using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkValueRepository<TContext, TValue>
    : IValueRepositorySave<TValue>
    where TContext : DbContext
    where TValue : class
{
    public async Task<bool> Save(TValue value, bool replace)
    {

        try
        {
            await Set.AddAsync(value);
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not save {typeof(TValue).Name}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
        
        return true;
    }

    public async Task<bool> SaveAll(IEnumerable<TValue> values,
        bool replace)
    {
        var changes = new List<ChangeEnum>();
        try
        {
            Set.AddRange(values);
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not save {typeof(TValue).Name} collection";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
        return true;
    }
}
