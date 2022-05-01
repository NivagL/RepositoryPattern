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
    , IKeyedRepositorySave<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    public async Task<Tuple<TKey, ChangeEnum>> KeyedSave(TValue value, bool replace)
    {
        var change = ChangeEnum.Updated;
        var key = default(TKey);
        if (KeyedModel.GetKey != null)
        {
            key = KeyedModel.GetKey(value);
        }
        TValue existing = null;
        try
        {
            if (!KeyedModel.IsKeyTuple)
            {
                existing = await Set.FindAsync(key);
            }
            else
            {
                var keys = new List<object>();
#pragma warning disable CS8604 // Possible null reference argument.
                keys.AddRange(TupleUtils.TupleToEnumerable(key));
#pragma warning restore CS8604 // Possible null reference argument.
                existing = await Set.FindAsync(keys.ToArray());
            }

            if (existing == null)
            {
                change = ChangeEnum.Added;
                await Set.AddAsync(value);
                await Context.SaveChangesAsync();
            }
            else
            {
                if (replace && (KeyedModel.Differ == null || KeyedModel.Differ(existing, value)))
                {
                    change = ChangeEnum.Updated;
                    if (KeyedModel.Assign != null)
                        KeyedModel.Assign(existing, value); //.Assign(value);
                    Set.Update(existing);
                    await Context.SaveChangesAsync();
                }
                else
                {
                    change = ChangeEnum.NoChange;
                }
            }
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not save {typeof(TValue).Name} for key {key}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            if(Logger.IsEnabled(LogLevel.Error))
                Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
        
        return new Tuple<TKey, ChangeEnum>(key, change);
    }

    public async Task<IEnumerable<Tuple<TKey, ChangeEnum>>> KeyedSaveAll(IEnumerable<TValue> values,bool replace)
    {
        var changes = new List<Tuple<TKey, ChangeEnum>>();
        try
        {
            if (!await Set.AnyAsync())
            {
                Set.AddRange(values);
                await Context.SaveChangesAsync();
            }
            else
            {
                foreach (var item in values)
                {
                    var save = await KeyedSave(item, replace);
                    changes.Add(Tuple.Create(save.Item1, save.Item2));
                }
                await Context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not save {typeof(TValue).Name} collection";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
        return changes;
    }
}
