using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;

namespace Repository.Test
{
    public class TestSimpleKeyRepositoryValues : TestRepository<SimpleKeyTestContext>
    {
        protected readonly IValueRepository<SimpleKeyTestModel> Repository;

        public TestSimpleKeyRepositoryValues()
            : base("SimpleKeyTest")
        {
            var model = new ValueModelFactory<SimpleKeyTestModel>(new SimpleKeyTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<ValueRepositoryFactory<SimpleKeyTestContext, SimpleKeyTestModel>>();
            var repository = new ValueRepositoryFactory<SimpleKeyTestContext, SimpleKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IValueRepository<SimpleKeyTestModel>>();
        }
    }
}
