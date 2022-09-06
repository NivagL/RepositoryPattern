using Configuration.Utility;
using EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;

namespace Repository.Test
{
    public class TestRepository<TContext>
        where TContext : DbContext
    {
        protected readonly UtilityBuilder UtilityBuilder;
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;

        public TestRepository(string configFolder)
        {
            UtilityBuilder = new UtilityBuilder()
            {
                ConfigurationFolder = configFolder
            };
            Configuration = UtilityBuilder.Configuration;
            Services = UtilityBuilder.Services;
            var provider = UtilityBuilder.Provider;
            var configurationValuePolicy = provider.GetRequiredService<IConfigurationValuePolicy>();

            var contextlogger = UtilityBuilder.Logger<IContextFactory>();
            var context = new ContextFactory<TContext>(
                Configuration, contextlogger, configurationValuePolicy);
            context.RegisterTypes(Services);
        }
    }
}
