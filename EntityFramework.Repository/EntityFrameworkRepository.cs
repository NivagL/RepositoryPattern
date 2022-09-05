using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository<TContext, TKey, TValue>
    : IRepository<TKey, TValue>, IConnected
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    protected readonly IConfiguration Configuration;
    protected readonly ILogger<IRepository<TKey, TValue>> Logger;
    protected readonly TContext Context;
    protected readonly DbSet<TValue> Set;
    protected readonly bool TrackQueries;
    protected readonly IKeyModel<TKey, TValue> Model;
    protected readonly string ConfigPath;

    public EntityFrameworkRepository(
        IConfiguration configuration, 
        ILogger<IRepository<TKey, TValue>> logger,
        TContext context, 
        IKeyModel<TKey, TValue> model, 
        bool trackQueries = false,
        string configPath = "")
    {
        Configuration = configuration;
        Logger = logger;
        Context = context;
        Model = model;
        Set = Context.Set<TValue>();
        TrackQueries = trackQueries;

        if(!string.IsNullOrWhiteSpace(configPath))
        {
            if (configPath.Last() == ':')
                ConfigPath = configPath;
            else
                ConfigPath = $"{configPath}:";
        }
    }

    private void SaveChanges()
    {
        if (Logger.IsEnabled(LogLevel.Information))
            Logger.LogInformation($"Saving {typeof(TValue).Name} changes");

        Context.SaveChanges();
    }

    public virtual IQueryable<TValue> Queryable(
        LoadFlagsEnum loadFlags)
    {
        return Context.Set<TValue>();
    }

    public virtual IQueryable<TValue> Queryable(
        Expression<Func<TValue, bool>> expression,
        LoadFlagsEnum loadFlags)
    {
        return Context.Set<TValue>().Where(expression);
    }

    public TimeSpan RequestTimeout
    {
        get 
        {
            if (string.IsNullOrEmpty(ConfigPath))
            {
                var configTimeout = Configuration.GetValue<TimeSpan>($"{ConfigPath}:Timeout");
                if (configTimeout != default(TimeSpan))
                    return configTimeout;

                return new TimeSpan(0, 0, 10);
            }
            else
                return Configuration.GetValue<TimeSpan>($"{ConfigPath}:Timeout");
        }
    }

//    public virtual void LoadRelated(
//        TValue value, LoadFlagsEnum loadFlags)
//    {
//    }

//#pragma warning disable CS1998
//    public virtual async Task LoadRelated(
//        IEnumerable<TValue> data, LoadFlagsEnum loadFlags)
//    {
//    }

//    public virtual async Task LoadRelated(
//        IDictionary<TKey, TValue> data, LoadFlagsEnum loadFlags)
//    {
//    }
//#pragma warning restore CS1998

//    public virtual void RemoveRelated(TValue value)
//    {
//    }

    public virtual bool IsConnected()
    {
        var connection = Context.Database.GetDbConnection();
        if (connection.State == ConnectionState.Broken)
        {
            return false;
        }

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            if (connection.State == ConnectionState.Open ||
                connection.State == ConnectionState.Connecting ||
                connection.State == ConnectionState.Executing ||
                connection.State == ConnectionState.Fetching)
            {
                Context.Database.ExecuteSqlRaw("SELECT 1");
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
