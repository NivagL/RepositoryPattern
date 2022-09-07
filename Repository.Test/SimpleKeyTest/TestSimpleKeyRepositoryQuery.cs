using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositoryQuery : TestSimpleKeyRepository
    {
        [TestMethod]
        public async Task SimpleKeyQueryTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var save = await Repository.Save(
                new SimpleGuidModel()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            ////Check we can query it
            //var query = await Repository.KeyedQuery(
            //    x => x.Date < DateTime.UtcNow,
            //    x => x.Date
            //);
            //Assert.IsTrue(query.Count > 1);
            //Assert.IsTrue(query.Keys.Contains(id));
        }

        [TestMethod]
        public async Task SimpleValueQueryTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var save = await Repository.Save(
                new SimpleGuidModel()
                {
                    Id = id,
                    Date = date,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can query it
            var query = await Repository.PagedQuery(
                x => x.Id == id, x => x.Id, new PageFilter());
            var any = query.Data.Values.Where(x => x.Id == id).Any();
            Assert.IsTrue(any);
        }
    }
}
