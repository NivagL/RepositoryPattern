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
                if (!Model.IsKeyTuple)
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
                    //RemoveRelated(existing);
                    Set.Remove(existing);
                    await Context.SaveChangesAsync();
                }

                return existing;
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} delete key timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, requestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} delete key error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }

    public async Task<int> DeleteAll(int chunks = 1000)
    {
        var requestTimeout = Configuration.GetValue<TimeSpan>($"{ConfigPath}Timeout");
        using (var cancellationToken = new CancellationTokenSource(requestTimeout))
        {
            try
            {
                var count = 0;
                while (await Set.AnyAsync())
                {
                    var chunk = await Set.Take(chunks).ToListAsync();
                    Set.RemoveRange(chunk);
                    await Context.SaveChangesAsync(cancellationToken.Token);
                    count += chunk.Count;
                }
                return count;
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} delete all timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, requestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} delete all error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }

    public async Task<int> DeleteQuery(Expression<Func<TValue, bool>> expression)
    {
        var requestTimeout = Configuration.GetValue<TimeSpan>($"{ConfigPath}Timeout");
        using (var cancellationToken = new CancellationTokenSource(requestTimeout))
        {
            try
            {
                var set = Set.Where(expression).ToList();
                var count = 0;
                while (set.Any())
                {
                    var chunk = await Set.Take(1000).ToListAsync();
                    Set.RemoveRange(chunk);
                    await Context.SaveChangesAsync(cancellationToken.Token);
                    count += chunk.Count;
                }
                return count;
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} delete all timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, requestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} delete all error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }
}
