using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Model;
using System;

namespace Repository.Test
{
    public class TestCompositeKeyRepository : TestRepository<CompositeKeyTestContext>
    {
        protected readonly IRepository<Tuple<Guid, DateTime>, CompositeKeyTestModel> Repository;

        public TestCompositeKeyRepository()
            : base("CompositeKeyTest")
        {
            var model = new ModelFactory<Tuple<Guid, DateTime>, CompositeKeyTestModel>(new CompositeKeyTestModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = DependencyBuilder.Logger<OneRepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyTestModel>>();
            var repository = new OneRepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = DependencyBuilder.Provider;
            Repository = provider.GetRequiredService<IRepository<Tuple<Guid, DateTime>, CompositeKeyTestModel>>();
        }
    }
}
