using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using System;
using System.Data;

namespace EntityFramework.Repository;

public partial class EntityFrameworkRepository
    : IConnected
{
    protected readonly IConfiguration Configuration;
    protected readonly ILogger<EntityFrameworkRepository> Logger;
    protected readonly DbContext Context;
    protected readonly bool TrackQueries;

    public EntityFrameworkRepository(IConfiguration configuration, 
        ILogger<EntityFrameworkRepository> logger,
        DbContext context, bool trackQueries = false)
    {
        Configuration = configuration;
        Logger = logger;
        Context = context;
        TrackQueries = trackQueries;
    }

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
