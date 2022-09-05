using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository<TContext, TKey, TValue>
    : IRepositorySave<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
//    public async Task<Tuple<TKey, ChangeEnum>> Save(TValue value, bool replace)
//    {
//        var change = ChangeEnum.Updated;
//        var key = default(TKey);
//        if (Model.GetKey != null)
//        {
//            key = Model.GetKey(value);
//        }
//        TValue existing = null;
//        try
//        {
//            if (!Model.IsKeyTuple)
//            {
//                existing = await Set.FindAsync(key);
//            }
//            else
//            {
//                var keys = new List<object>();
//#pragma warning disable CS8604 // Possible null reference argument.
//                keys.AddRange(TupleUtils.TupleToEnumerable(key));
//#pragma warning restore CS8604 // Possible null reference argument.
//                existing = await Set.FindAsync(keys.ToArray());
//            }

//            if (existing == null)
//            {
//                change = ChangeEnum.Added;
//                await Set.AddAsync(value);
//                await Context.SaveChangesAsync();
//            }
//            else
//            {
//                if (replace && (Model.Differ == null || Model.Differ(existing, value)))
//                {
//                    change = ChangeEnum.Updated;
//                    if (Model.Assign != null)
//                        Model.Assign(existing, value); //.Assign(value);
//                    Set.Update(existing);
//                    await Context.SaveChangesAsync();
//                }
//                else
//                {
//                    change = ChangeEnum.NoChange;
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            var userMsg = $"Repository could not save {typeof(TValue).Name} for key {key}";
//            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
//            if(Logger.IsEnabled(LogLevel.Error))
//                Logger.LogError($"{userMsg} - {msg}");

//            throw new RepositoryException(userMsg, msg);
//        }
        
//        return new Tuple<TKey, ChangeEnum>(key, change);
//    }

    public async Task<IEnumerable<Tuple<TKey, ChangeEnum>>> Save(IEnumerable<TValue> values
        , bool replace = true, bool trackChanges = false)
    {
        try
        {
            //The cheap versions...
            if(replace && !trackChanges)
            {
                await Set.AddRangeAsync(values);
                await Context.SaveChangesAsync();

                return Enumerable.Repeat(
                    Tuple.Create(default(TKey), ChangeEnum.NoChange)
                        , values.Count());
            }

            //The more expensive version...
            var changes = new List<Tuple<TKey, ChangeEnum>>();
            foreach (var item in values)
            {
                var save = await Save(item, replace, trackChanges);
                changes.Add(Tuple.Create(save.Item1, save.Item2));
            }
            return changes;
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not save {typeof(TValue).Name} collection";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }

    public async Task<Tuple<TKey, ChangeEnum>> Save(TValue value, 
        bool replace = true, bool trackChanges = false)
    {

        try
        {
            bool any = false;
            var key = Model.GetKey(value);
            
            if (!replace || trackChanges)
                any = await Any(key);

            if (!replace && any)
                return Tuple.Create(key, ChangeEnum.NoChange);

            await Set.AddAsync(value);
            await Context.SaveChangesAsync();

            var change = any ? ChangeEnum.Added : ChangeEnum.Updated;
            return Tuple.Create(key, change);
        }
        catch (Exception ex)
        {
            var userMsg = $"Repository could not save {typeof(TValue).Name}";
            var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
            Logger.LogError($"{userMsg} - {msg}");

            throw new RepositoryException(userMsg, msg);
        }
    }

    //public async Task<bool> SaveAll(IEnumerable<TValue> values,
    //    bool replace)
    //{
    //    var changes = new List<ChangeEnum>();
    //    try
    //    {
    //        Set.AddRange(values);
    //        await Context.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        var userMsg = $"Repository could not save {typeof(TValue).Name} collection";
    //        var msg = $"{ex.Message} {ex.StackTrace} {ex.InnerException?.ToString()}";
    //        Logger.LogError($"{userMsg} - {msg}");

    //        throw new RepositoryException(userMsg, msg);
    //    }
    //    return true;
    //}
}
