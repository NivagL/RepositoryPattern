using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using Repository.Test.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                new SimpleKeyTestModel()
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
                new SimpleKeyTestModel()
                {
                    Id = id,
                    Date = date,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can query it
            var earliest = date - new TimeSpan(0, 1, 0, 0);
            var load = await Repository.Load(id);
            Assert.IsTrue(load.Id == id);
        }
    }
}
