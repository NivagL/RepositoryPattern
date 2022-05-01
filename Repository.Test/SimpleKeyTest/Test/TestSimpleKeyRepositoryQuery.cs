using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            //Check we can query it
            var query = await Repository.KeyedQuery(
                x => x.Date < DateTime.UtcNow,
                x => x.Date
            );
            Assert.IsTrue(query.Count > 1);
            Assert.IsTrue(query.Keys.Contains(id));
        }
    }
}
