using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository<TContext, TKey, TValue>
    : IRepositoryAny<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    public async Task<bool> Any()
    {
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                return await Set.AnyAsync(cancellationToken.Token);
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} any check timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, RequestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} any check error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }

    public async Task<bool> Any(TKey key)
    {
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                //does this load into memory or get evaluated on databse server?
                //var item = await Set.AnyAsync(x => KeyedModel.KeysEqual(KeyedModel.GetKey(x), key));

                if (!Model.IsKeyTuple)
                {
                    var existing = await Set.FindAsync(new object[] { key }, cancellationToken.Token);
                    return existing != null;
                }
                else
                {
                    var keys = new List<object>();
                    keys.AddRange(TupleUtils.TupleToEnumerable(key));
                    var existing = await Set.FindAsync(keys.ToArray(), cancellationToken.Token);
                    return existing != null;
                }
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} any check timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, RequestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} any check error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }

    public async Task<bool> Any(Expression<Func<TValue, bool>> expression)
    {
        using (var cancellationToken = new CancellationTokenSource(RequestTimeout))
        {
            try
            {
                var queryable = Queryable(expression, LoadFlagsEnum.All);
                if(TrackQueries)
                    return await queryable.AnyAsync(cancellationToken.Token);
                else
                    return await queryable.AsNoTracking().AnyAsync(cancellationToken.Token);
            }
            catch (OperationCanceledException ex)
            {
                var userMsg = $"{nameof(TValue)} any with query check timed out";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryExceptionTimeout(
                    userMsg, systemMsg, RequestTimeout);
            }
            catch (Exception ex)
            {
                var userMsg = $"{nameof(TValue)} any with query check error";
                var systemMsg = $"{ex.Message}/{ex.StackTrace}/{ex.InnerException?.Message}";

                if (Logger.IsEnabled(LogLevel.Error))
                    Logger.LogError($"{userMsg}/{systemMsg}");

                throw new RepositoryException(
                    userMsg, systemMsg);
            }
        }
    }
}
