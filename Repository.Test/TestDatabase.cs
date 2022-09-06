using Configuration.Utility;
using EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;

namespace Repository.Test
{
    public class TestDatabase<TContext>
        where TContext : DbContext
    {
        protected readonly UtilityBuilder UtilityBuilder;
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;
        protected readonly IDatabaseInitialiser DatabaseInitialiser;

        public TestDatabase(string configPath)
        {
            UtilityBuilder = new UtilityBuilder()
            {
                ConfigurationFolder = configPath
            };
            Configuration = UtilityBuilder.Configuration;
            Services = UtilityBuilder.Services;
            var provider = UtilityBuilder.Provider;
            var configurationValuePolicy = provider.GetRequiredService<IConfigurationValuePolicy>();

            var contextlogger = UtilityBuilder.Logger<IContextFactory>();
            var context = new ContextFactory<TContext>(
                Configuration, contextlogger, configurationValuePolicy);
            context.RegisterTypes(Services);

            var databaselogger = UtilityBuilder.Logger<IDatabaseInitialiser>();
            DatabaseInitialiser = new DatabaseInitialiser<TContext>(
                Configuration, databaselogger, configurationValuePolicy);
        }
    }
}
