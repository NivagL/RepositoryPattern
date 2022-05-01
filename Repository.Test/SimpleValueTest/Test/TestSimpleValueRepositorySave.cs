using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleValueRepositorySave : TestSimpleValueRepository
    {
        [TestInitialize]
        public void Initialise()
        {
            Assert.Inconclusive("Entity framework does not supports non-keyed entities!");
        }

        [TestMethod]
        public async Task NoKeySimpleValueSaveTest()
        {
            Assert.Inconclusive("Entity framework does not supports non-keyed entities!");

            var save = await Repository.Save(
                new SimpleValueTestModel()
                {
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save);
        }
    }
}
