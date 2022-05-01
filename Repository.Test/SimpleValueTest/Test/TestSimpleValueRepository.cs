using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using Repository.Model;

namespace Repository.Test
{
    public class TestSimpleValueRepository : TestRepository<SimpleValueTestContext>
    {
        protected readonly IValueRepository<SimpleValueTestModel> Repository;

        public TestSimpleValueRepository()
            : base("SimpleValueTest")
        {
            var model = new ValueModelFactory<SimpleValueTestModel>(new SimpleValueTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<ValueRepositoryFactory<SimpleValueTestContext, SimpleValueTestModel>>();
            var repository = new ValueRepositoryFactory<SimpleValueTestContext, SimpleValueTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IValueRepository<SimpleValueTestModel>>();
        }
    }
}
