using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleValueRepositoryQuery : TestSimpleValueRepository
    {
        [TestInitialize]
        public void Initialise()
        {
            Assert.Inconclusive("Entity framework does not supports non-keyed entities!");
        }

        [TestMethod]
        public async Task NoKeySimpleValueQueryTest()
        {
            //Save one
            var save = await Repository.Save(
                new SimpleValueTestModel()
                {
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
        }
    }
}
