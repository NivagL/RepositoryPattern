using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace EntityFramework.Repository;

public class ValueRepositoryFactory<TContext, TValue> 
    : IValueRepositoryFactory<TValue> 
    where TContext : DbContext
    where TValue : class
{
    private readonly IConfiguration Configuration;
    private readonly ILogger<IValueRepositoryFactory<TValue>> Logger;
    public Func<IServiceProvider, IValueRepository<TValue>> Repository { get; set; }
    public Func<IServiceProvider, IValueRepositoryAny<TValue>> RepositoryAny { get; set; }
    public Func<IServiceProvider, IValueRepositoryLoad<TValue>> RepositoryLoad { get; set; }
    public Func<IServiceProvider, IValueRepositorySave<TValue>> RepositorySave { get; set; }
    public Func<IServiceProvider, IValueRepositoryQuery<TValue>> RepositoryQuery { get; set; }
    public Func<IServiceProvider, IValueRepositoryDelete<TValue>> RepositoryDelete { get; set; }

    public ValueRepositoryFactory(IConfiguration configuration, 
        ILogger<IValueRepositoryFactory<TValue>> logger)
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

    public EntityFrameworkValueRepository<TContext, TValue> CreateRepository(IServiceProvider serviceProvider)
    {
        try
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<EntityFrameworkValueRepository<TContext, TValue>>();
            var context = serviceProvider.GetRequiredService<TContext>();
            var model = serviceProvider.GetRequiredService<IValueModel<TValue>>();

            return new EntityFrameworkValueRepository<TContext, TValue>(
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
