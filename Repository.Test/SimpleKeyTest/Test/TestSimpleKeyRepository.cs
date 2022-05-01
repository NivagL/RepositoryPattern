using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace Repository.Test
{
    public class TestSimpleKeyRepository : TestRepository<SimpleKeyTestContext>
    {
        protected readonly IKeyedRepository<Guid, SimpleKeyTestModel> Repository;

        public TestSimpleKeyRepository()
            : base("SimpleKeyTest")
        {
            var model = new KeyedModelFactory<Guid, SimpleKeyTestModel>(new SimpleKeyTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<KeyedRepositoryFactory<SimpleKeyTestContext, Guid, SimpleKeyTestModel>>();
            var repository = new KeyedRepositoryFactory<SimpleKeyTestContext, Guid, SimpleKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IKeyedRepository<Guid, SimpleKeyTestModel>>();
        }
    }
}
