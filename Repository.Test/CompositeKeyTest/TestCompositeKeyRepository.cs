using EntityFramework.Repository;
using Microsoft.Extensions.DependencyInjection;
using Model.Abstraction;
using Repository.Abstraction;
using System;
using Test.Model;

namespace Repository.Test
{
    public class TestCompositeKeyRepository : TestRepository<CompositeKeyTestContext>
    {
        protected readonly IRepository<Tuple<Guid, DateTime>, CompositeKeyModel> Repository;

        public TestCompositeKeyRepository()
            : base("CompositeKeyTest")
        {
            var model = new ModelFactory<Tuple<Guid, DateTime>, CompositeKeyModel>(new CompositeKeyModelMeta());
            model.RegisterTypes(Services);

            var repositorylogger = UtilityBuilder.Logger<RepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyModel>>();
            var repository = new RepositoryFactory<CompositeKeyTestContext, Tuple<Guid, DateTime>, CompositeKeyModel>(Configuration, repositorylogger);
            repository.RegisterTypes(Services);

            var provider = UtilityBuilder.Provider;
            Repository = provider.GetRequiredService<IRepository<Tuple<Guid, DateTime>, CompositeKeyModel>>();
        }
    }
}
