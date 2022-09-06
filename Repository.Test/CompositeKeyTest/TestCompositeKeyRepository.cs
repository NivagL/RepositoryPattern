using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Model.Abstraction;
using Repository.Abstraction;
using Repository.Test.Model;
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

            var repositorylogger = UtilityBuilder.Logger<RepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyTestModel>>();
            var repository = new RepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyTestModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = UtilityBuilder.Provider;
            Repository = provider.GetRequiredService<IRepository<Tuple<Guid, DateTime>, CompositeKeyTestModel>>();
        }
    }
}
