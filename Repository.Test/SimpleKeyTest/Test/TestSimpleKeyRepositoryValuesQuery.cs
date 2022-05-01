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
            Assert.IsTrue(save);

            //Check we can query it
            var earliest = date - new TimeSpan(0, 1, 0, 0);
            var query = await Repository.Query(
                x => x.Date > earliest,
                x => x.Date
            );
            Assert.IsTrue(query.Any());
            Assert.IsTrue(query.Where(X => X.Id == id).Any());
        }
    }
}
