using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;

namespace EntityFramework.Repository.Mock;

/// <summary>
/// Registers the entity framework in-memory content for testing
/// </summary>
/// <typeparam name="TContext">Your concrete entity framework context</typeparam>

public class InMemoryContextFactory<TContext> : IContextFactory
    where TContext : DbContext
{
    private readonly static int DefaultTimeout = 30;
    private readonly static bool DefaultConnectionPool = true;

    private readonly IConfiguration Configuration;
    private readonly ILogger<IContextFactory> Logger;

    public string Application { get; set; }
    public string Database { get; set; }
    public string Connection { get; set; }
    public int Timeout { get; set; }
    public bool Pool { get; set; }

    public InMemoryContextFactory(IConfiguration configuration, 
        ILogger<IContextFactory> logger, string configPath = "")
    {
        Configuration = configuration;
        Logger = logger;

        Application = GetConfigurationValue(configPath, "Service", "");
        if (string.IsNullOrEmpty(Application))
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError("Could not find service configuration");

            throw new RepositoryExceptionConfiguration("Service");
        }

        Database = GetConfigurationValue(configPath, "Database", "");
        if (string.IsNullOrEmpty(Database))
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError("Could not find database configuration");

            throw new RepositoryExceptionConfiguration("Database");
        }

        Timeout = GetConfigurationValue(configPath, "CommandTimeout", DefaultTimeout);
        Pool = GetConfigurationValue(configPath, "ContextPool", DefaultConnectionPool);

        Connection = "";
    }

    private T GetConfigurationValue<T>(string configPath, string name, T defaultValue)
        where T : IEquatable<T>
    {
        var value = default(T);

        if (!string.IsNullOrEmpty(configPath))
            value = Configuration.GetValue<T>($"{configPath}:{name}");

        if (value == null || value.Equals(default(T)))
            value = Configuration.GetValue<T>($"Database:{name}");

        if (value == null || value.Equals(default(T)))
            value = Configuration.GetValue<T>($"{name}");

        if (value == null || value.Equals(default(T)))
            value = defaultValue;

        return value;
    }

    public void RegisterTypes(IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

        Logger.LogInformation("Initialising {0} service context {1} connection {2} timeout {3}",
            Application, Database, Connection, Timeout);

        services.AddDbContext<TContext>(options =>
        {
            options.UseLoggerFactory(loggerFactory);
            options.UseInMemoryDatabase(Database);
            options.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }, ServiceLifetime.Scoped);
    }
}
