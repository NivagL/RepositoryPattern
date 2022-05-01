using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleValueRepositoryLoad : TestSimpleValueRepository
    {
        [TestInitialize]
        public void Initialise()
        {
            Assert.Inconclusive("Entity framework does not supports non-keyed entities!");
        }

        [TestMethod]
        public async Task NoKeySimpleValueLoadTest()
        {
            //Delete them
            var delete = await Repository.DeleteAll();

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

            //Check we can load it - no keys so load all
            var load = await Repository.LoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1 },
                x => x.Date
            );
        }

        [TestMethod]
        public async Task NoKeySimpleValuePageLoadTest()
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

            //Check we can load it
            var load = await Repository.LoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1 },
                x => x.Date
                );
        }
    }
}
