using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestCompositeKeyRepositoryQuery : TestCompositeKeyRepository
    {
        [TestMethod]
        public async Task CompositeKeyQueryTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var save = await Repository.KeyedSave(
                new CompositeKeyTestModel()
                {
                    Id = id,
                    Date = date,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item1.Item1 == id
                && save.Item1.Item2 == date);

            //Check we can query it
            var query = await Repository.KeyedQuery(
                x => x.Date < DateTime.UtcNow,
                x => x.Date
            );
            Assert.IsTrue(query.Count > 1);
            Assert.IsTrue(query.Keys.Contains(Tuple.Create(id, date)));
        }
    }
}
