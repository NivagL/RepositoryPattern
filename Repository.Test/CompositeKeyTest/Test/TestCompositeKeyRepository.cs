using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace Repository.Test
{
    public class TestCompositeKeyRepository : TestRepository<CompositeKeyTestContext>
    {
        protected readonly IKeyedRepository<Tuple<Guid, DateTime>, CompositeKeyTestModel> Repository;

        public TestCompositeKeyRepository()
            : base("CompositeKeyTest")
        {
            var model = new KeyedModelFactory<Tuple<Guid, DateTime>, CompositeKeyTestModel>(new CompositeKeyTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<KeyedRepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyTestModel>>();
            var repository = new KeyedRepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IKeyedRepository<Tuple<Guid, DateTime>, CompositeKeyTestModel>>();
        }
    }
}
