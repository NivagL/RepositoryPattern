using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;

namespace Repository.Test
{
    public class TestCompositeKeyRepositoryValues : TestRepository<CompositeKeyTestContext>
    {
        protected readonly IValueRepository<CompositeKeyTestModel> Repository;

        public TestCompositeKeyRepositoryValues()
            : base("CompositeKeyTest")
        {
            var model = new ValueModelFactory<CompositeKeyTestModel>(new CompositeKeyTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<ValueRepositoryFactory<CompositeKeyTestContext, CompositeKeyTestModel>>();
            var repository = new ValueRepositoryFactory<CompositeKeyTestContext, CompositeKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IValueRepository<CompositeKeyTestModel>>();
        }
    }
}
