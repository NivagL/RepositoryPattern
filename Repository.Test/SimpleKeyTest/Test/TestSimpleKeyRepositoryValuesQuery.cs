using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositoryValuesQuery : TestSimpleKeyRepositoryValues
    {
        [TestMethod]
        public async Task SimpleValueQueryTest()
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
            Assert.IsTrue(save);

            //Check we can query it
            var query = await Repository.Query(
                x => x.Date < DateTime.UtcNow,
                x => x.Date
            );
            Assert.IsTrue(query.Any());
            Assert.IsTrue(query.Where(x => x.Id == id).Any());
        }
    }
}
