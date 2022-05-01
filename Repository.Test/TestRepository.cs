using EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace Repository.Test
{
    public class TestRepository<TContext>
        where TContext : DbContext
    {
        protected readonly DependencyBuilder DependencyBuilder;
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;

        public TestRepository(string configFolder)
        {
            DependencyBuilder = new DependencyBuilder()
            {
                ConfigurationFolder = configFolder
            };
            Configuration = DependencyBuilder.Configuration;
            Services = DependencyBuilder.Services;
            var provider = DependencyBuilder.Provider;
            var configurationValuePolicy = provider.GetRequiredService<IConfigurationValuePolicy>();

            var contextlogger = DependencyBuilder.Logger<IContextFactory>();
            var context = new ContextFactory<TContext>(
                Configuration, contextlogger, configurationValuePolicy);
            context.RegisterTypes(Services);
        }
    }
}
