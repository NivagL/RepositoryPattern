using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Model.Abstraction;
using Repository.Abstraction;
using Repository.Test.Model;
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

            var repositorylogger = UtilityBuilder.Logger<RepositoryFactory<SimpleKeyTestContext, Guid, SimpleKeyTestModel>>();
            var repository = new RepositoryFactory<SimpleKeyTestContext, Guid, SimpleKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = UtilityBuilder.Provider;
            Repository = provider.GetRequiredService<IRepository<Guid, SimpleKeyTestModel>>();
        }
    }
}
