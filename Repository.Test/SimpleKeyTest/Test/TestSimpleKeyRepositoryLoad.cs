using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositoryLoad : TestSimpleKeyRepository
    {
        [TestMethod]
        public async Task SimpleKeyLoadTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var save = await Repository.KeyedSave(
                new SimpleKeyTestModel()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item1 == id);

            //Check we can load it
            var load = await Repository.KeyedLoad(id);
            Assert.IsTrue(load.Id == id);
        }

        [TestMethod]
        public async Task SimpleKeyPageLoadTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var save = await Repository.KeyedSave(
                new SimpleKeyTestModel()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item1 == id);

            //Check we can load it
            var load = await Repository.KeyedLoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1},
                x => x.Date
                );

            Assert.IsTrue(load.Data.ContainsKey(id));
        }
    }
}
