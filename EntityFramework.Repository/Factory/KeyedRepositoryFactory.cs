using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace EntityFramework.Repository;

public class KeyedRepositoryFactory<TContext, TKey, TValue> 
    : IKeyedRepositoryFactory<TKey, TValue> 
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    private readonly IConfiguration Configuration;
    private readonly ILogger<IKeyedRepositoryFactory<TKey, TValue>> Logger;
    public Func<IServiceProvider, IKeyedRepository<TKey, TValue>> Repository { get; set; }
    public Func<IServiceProvider, IKeyedRepositoryAny<TKey, TValue>> RepositoryAny { get; set; }
    public Func<IServiceProvider, IKeyedRepositoryLoad<TKey, TValue>> RepositoryLoad { get; set; }
    public Func<IServiceProvider, IKeyedRepositorySave<TKey, TValue>> RepositorySave { get; set; }
    public Func<IServiceProvider, IValueRepositoryQuery<TValue>> RepositoryQuery { get; set; }
    public Func<IServiceProvider, IKeyedRepositoryDelete<TKey, TValue>> RepositoryDelete { get; set; }

    public KeyedRepositoryFactory(IConfiguration configuration, 
        ILogger<IKeyedRepositoryFactory<TKey, TValue>> logger)
    {
        Configuration = configuration;
        Logger = logger;

        Repository = CreateRepository;
        RepositoryAny = CreateRepository;
        RepositoryLoad = CreateRepository;
        RepositorySave = CreateRepository;
        RepositoryQuery = CreateRepository;
        RepositoryDelete = CreateRepository;
    }

    public void RegisterTypes(IServiceCollection services)
    {
        if (Repository != null)
            services.AddScoped(Repository);

        if (RepositoryAny != null)
            services.AddScoped(RepositoryAny);

        if (RepositoryLoad != null)
            services.AddScoped(RepositoryLoad);

        if (RepositorySave != null)
            services.AddScoped(RepositorySave);

        if (RepositoryQuery != null)
            services.AddScoped(RepositoryQuery);

        if (RepositoryDelete != null)
            services.AddScoped(RepositoryDelete);
    }

    public EntityFrameworkKeyedRepository<TContext, TKey, TValue> CreateRepository(IServiceProvider serviceProvider)
    {
        try
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<EntityFrameworkKeyedRepository<TContext, TKey, TValue>>();
            var context = serviceProvider.GetRequiredService<TContext>();
            var model = serviceProvider.GetRequiredService<IKeyModel<TKey, TValue>>();

            return new EntityFrameworkKeyedRepository<TContext, TKey, TValue>(
                configuration, logger, context, model);
        }
        catch (Exception ex)
        {
            Logger.LogError("Tools Factory Creating Model Repository {0} {1}/{2}",
            typeof(TValue).Name, ex.Message, ex.StackTrace);
            throw;
        }
    }
}
