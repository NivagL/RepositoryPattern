using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace Repository.Test
{
    public class TestSimpleKeyRepository : TestRepository<SimpleKeyTestContext>
    {
        protected readonly IRepository<Guid, SimpleKeyTestModel> Repository;

        public TestSimpleKeyRepository()
            : base("SimpleKeyTest")
        {
            var model = new ModelFactory<Guid, SimpleKeyTestModel>(new SimpleKeyTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<OneRepositoryFactory<SimpleKeyTestContext, Guid, SimpleKeyTestModel>>();
            var repository = new OneRepositoryFactory<SimpleKeyTestContext, Guid, SimpleKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IRepository<Guid, SimpleKeyTestModel>>();
        }
    }
}
