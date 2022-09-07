using Configuration.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;

namespace EntityFramework.Repository;

/// <summary>
/// Registers the entity framework context
/// </summary>
/// <typeparam name="TContext">Your concrete entity framework context</typeparam>

public class ContextFactory<TContext> : IContextFactory
    where TContext : DbContext
{
    private readonly static int DefaultTimeout = 30;
    private readonly static bool DefaultConnectionPool = true;

    private readonly IConfiguration Configuration;
    private readonly ILogger<IContextFactory> Logger;
    private readonly string ConfigPath;
    private readonly IConfigurationValuePolicy ConfigurationValuePolicy;

    public string Application { get; set; }
    public string Database { get; set; }
    public string Connection { get; set; }
    public int Timeout { get; set; }
    public bool Pool { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <param name="configPath"></param>
    /// <exception cref="RepositoryExceptionConfiguration"></exception>
    public ContextFactory(
        IConfiguration configuration,
        ILogger<IContextFactory> logger,
        IConfigurationValuePolicy configurationValuePolicy,
        string configPath = ""
        )
    {
        Configuration = configuration;
        Logger = logger;
        ConfigurationValuePolicy = configurationValuePolicy;
        ConfigPath = configPath;

        Application = ConfigurationValuePolicy.GetValue(ConfigPath, "Application", "Name", "");
        if (string.IsNullOrEmpty(Application))
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError("Could not find configuration");

            throw new RepositoryExceptionConfiguration("Application");
        }

        Database = ConfigurationValuePolicy.GetValue(ConfigPath, "Database", "Name", "");
        if (string.IsNullOrEmpty(Database))
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError("Could not find database configuration");

            throw new RepositoryExceptionConfiguration("Database");
        }

        Timeout = ConfigurationValuePolicy.GetValue(ConfigPath, "Database", "CommandTimeout", DefaultTimeout);
        Pool = ConfigurationValuePolicy.GetValue(ConfigPath, "Database", "ContextPool", DefaultConnectionPool);

        Connection = GetConnection(configPath, Database);
        if (string.IsNullOrEmpty(Connection))
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError("Could not find database connection configuration");

            throw new RepositoryExceptionConfiguration("Connection");
        }
    }

    private string GetConnection(string configPath, string database)
    {
        var connection = ConfigurationValuePolicy.GetValue(configPath, "", database, "");
        if (string.IsNullOrEmpty(connection))
        {
            connection = Configuration.GetConnectionString(database);

            if (string.IsNullOrEmpty(connection))
                connection = Configuration.GetValue<string>($"{configPath}:ConnectionStrings--{database}");

            if (string.IsNullOrEmpty(connection))
                connection = Configuration.GetValue<string>($"ConnectionStrings--{database}");
        }

        return connection;
    }

    public void RegisterTypes(IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

        if (Pool)
        {
            if (Logger.IsEnabled(LogLevel.Information))
                Logger.LogInformation("Initialising {0} pooled database context {1} timeout {2}",
                    Application, Database, Timeout);

            services.AddDbContextPool<TContext>(options =>
            {
                options.UseLoggerFactory(loggerFactory);
                //options.UseInternalServiceProvider(provider);
                options.UseSqlServer(Connection,
                    sqlServerOptions => sqlServerOptions.CommandTimeout(Timeout));
                //options.EnableSensitiveDataLogging();
            });
        }
        else
        {
            if (Logger.IsEnabled(LogLevel.Information))
                Logger.LogInformation("Initialising {0} database context {1} timeout {2}",
                    Application, Database, Timeout);

            services.AddDbContext<TContext>(options =>
            {
                options.UseLoggerFactory(loggerFactory);
                options.UseSqlServer(Connection,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.CommandTimeout(Timeout);
                        sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sqlServerOptions.EnableRetryOnFailure(); 
                    });
                //options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Scoped);
        }
    }
}
