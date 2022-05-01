using EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;

namespace Repository.Test
{
    public class TestDatabase<TContext>
        where TContext : DbContext
    {
        protected readonly DependencyBuilder DependencyBuilder;
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;
        protected readonly IDatabaseInitialiser DatabaseInitialiser;

        public TestDatabase(string configPath)
        {
            DependencyBuilder = new DependencyBuilder()
            {
                ConfigurationFolder = configPath
            };
            Configuration = DependencyBuilder.Configuration;
            Services = DependencyBuilder.Services;
            var provider = DependencyBuilder.Provider;
            var configurationValuePolicy = provider.GetRequiredService<IConfigurationValuePolicy>();

            var contextlogger = DependencyBuilder.Logger<IContextFactory>();
            var context = new ContextFactory<TContext>(
                Configuration, contextlogger, configurationValuePolicy);
            context.RegisterTypes(Services);

            var databaselogger = DependencyBuilder.Logger<IDatabaseInitialiser>();
            DatabaseInitialiser = new DatabaseInitialiser<TContext>(
                Configuration, databaselogger, configurationValuePolicy);
        }
    }
}
