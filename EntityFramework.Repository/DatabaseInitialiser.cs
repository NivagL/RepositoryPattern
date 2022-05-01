using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Model;
using System;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public class DatabaseInitialiser<TContext> : IDatabaseInitialiser
    where TContext : DbContext
{
    private readonly IConfiguration Configuration;
    private readonly ILogger<IDatabaseInitialiser> Logger;
    private readonly IConfigurationValuePolicy ConfigurationValuePolicy;
    private readonly string ConfigPath;

    public DatabaseInitialiser(
        IConfiguration configuration, 
        ILogger<IDatabaseInitialiser> logger,
        IConfigurationValuePolicy configurationValuePolicy,
        string configPath = ""
        )
    {
        Configuration = configuration;
        Logger  = logger;
        ConfigPath = configPath;
        ConfigurationValuePolicy = configurationValuePolicy;
    }

    public async Task<bool> InitialiseDatabase(IServiceProvider serviceProvider)
    {
        if(Logger.IsEnabled(LogLevel.Information))
            Logger.LogInformation($"Initialising database {nameof(TContext)}");

        try
        {
            var context = serviceProvider.GetRequiredService<TContext>();

            var drop = await DropDatebase(context);
            var create = await CreateDatabase(context);
            var tables = await CreateTables(context);
            //RefreshNonTableObjects();

            return drop && create && tables;
        }
        catch (Exception ex)
        {
            if(Logger.IsEnabled(LogLevel.Error))
                Logger.LogError($"Failed to initialise database {ex.Message}/{ex.StackTrace}");

            throw;
        }
    }

    public async Task<bool> DropDatebase(TContext context)
    {
        try
        {
            bool dropDatabase = ConfigurationValuePolicy.GetValue(ConfigPath, "Database", "DropDatabase", false);
            if (dropDatabase)
            {
                if(Logger.IsEnabled(LogLevel.Information))
                    Logger.LogInformation("Dropping database as per configuration.");
                
                return await context.Database.EnsureDeletedAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            if(Logger.IsEnabled(LogLevel.Error))
                Logger.LogError($"Error dropping database {ex.Message}/{ex.StackTrace}");

            throw new RepositoryExceptionDatabaseDrop(nameof(TContext), ex);
        }
    }

    public async Task<bool> CreateDatabase(TContext context)
    {
        try
        {
            bool createDatabase = ConfigurationValuePolicy.GetValue(ConfigPath, "Database", "CreateDatabase", false);
            if (createDatabase)
            {
                if(Logger.IsEnabled(LogLevel.Information))
                    Logger.LogInformation("Creating database as per configuration.");

                return await context.Database.EnsureCreatedAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            if(Logger.IsEnabled(LogLevel.Error))
                Logger.LogError($"Error creating database {ex.Message}/{ex.StackTrace}");

            throw new RepositoryExceptionDatabaseDrop(nameof(TContext), ex);
        }
    }

    public async Task<bool> CreateTables(TContext context)
    {
        try
        {
            bool createTables = ConfigurationValuePolicy.GetValue(ConfigPath, "Database", "CreateTables", false);
            if (createTables)
            {
                if(Logger.IsEnabled(LogLevel.Information))
                    Logger.LogInformation("Creating database tables as per configuration.");

                RelationalDatabaseCreator databaseCreator =
                    (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                
                await databaseCreator.CreateTablesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            if(Logger.IsEnabled(LogLevel.Error))
                Logger.LogError($"Error creating database tables {ex.Message}/{ex.StackTrace}");

            throw new RepositoryExceptionDatabaseTables(nameof(TContext), ex);
        }
    }
}
