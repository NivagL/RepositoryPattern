using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Model.Abstraction;
using Repository.Abstraction;
using Test.Model;
using System;

namespace Repository.Test
{
    public class TestSimpleKeyRepository : TestRepository<SimpleKeyTestContext>
    {
        protected readonly IRepository<Guid, SimpleGuidModel> Repository;

        public TestSimpleKeyRepository()
            : base("SimpleKeyTest")
        {
            var model = new ModelFactory<Guid, SimpleGuidModel>(new SimpleGuidModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = UtilityBuilder.Logger<RepositoryFactory<SimpleKeyTestContext, Guid, SimpleGuidModel>>();
            var repository = new RepositoryFactory<SimpleKeyTestContext, Guid, SimpleGuidModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = UtilityBuilder.Provider;
            Repository = provider.GetRequiredService<IRepository<Guid, SimpleGuidModel>>();
        }
    }
}
