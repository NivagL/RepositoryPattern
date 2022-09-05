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
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                TValue existing = null;
                if (!Model.IsKeyTuple)
                {
                    existing = await Set.FindAsync(new object[] { key }, cancellationToken.Token);
                }
                else
                {
                    var keys = new List<object>();
                    keys.AddRange(TupleUtils.TupleToEnumerable(key));
                    existing = await Set.FindAsync(keys.ToArray(), cancellationToken.Token);
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
                    userMsg, systemMsg, RequestTimeout);
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
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                var count = 0;
                while (await Set.AnyAsync(cancellationToken.Token))
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
                    userMsg, systemMsg, RequestTimeout);
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

    public async Task<int> DeleteQuery(Expression<Func<TValue, bool>> expression
        , int chunks = 1000)
    {
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                var count = 0;
                var set = Set.Where(expression).Take(chunks).ToList();
                while (set.Any())
                {
                    Set.RemoveRange(set);
                    await Context.SaveChangesAsync(cancellationToken.Token);
                    count += set.Count();
                    set = Set.Where(expression).Take(chunks).ToList();
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
                    userMsg, systemMsg, RequestTimeout);
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
