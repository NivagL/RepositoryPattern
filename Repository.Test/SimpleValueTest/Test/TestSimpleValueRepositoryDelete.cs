using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleValueRepositoryDelete : TestSimpleValueRepository
    {
        [TestInitialize]
        public void Initialise()
        {
            Assert.Inconclusive("Entity framework does not supports non-keyed entities!");
        }

        [TestMethod]
        public async Task NoKeySimpleValueDeleteAllTest()
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

            //Check we can delete it - no keys so delete all
            var deleted = await Repository.DeleteAll();
            Assert.IsTrue(deleted);

            //Check it's no longer there
            var any = await Repository.Any();
            Assert.IsTrue(!any);
        }

        [TestMethod]
        public async Task NoKeySimpleValueDeleteExpressionTest()
        {
            //Save one
            var save1 = await Repository.Save(
                new SimpleValueTestModel()
                {
                    Date = DateTime.UtcNow - new TimeSpan(1, 0, 0, 0),
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save1);

            //Save another
            var save2 = await Repository.Save(
                new SimpleValueTestModel()
                {
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save2);

            var earliest = DateTime.UtcNow - new TimeSpan(0, 1, 0, 0);
            //Check we can delete it - no keys so delete all
            var deleted = await Repository.DeleteQuery(
                x => x.Date < earliest);
            Assert.IsTrue(deleted > 0);

            //Check it's no longer there
            var any = await Repository.Any();
            Assert.IsTrue(any);
        }
    }
}
