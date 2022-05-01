using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositoryValuesDelete : TestSimpleKeyRepositoryValues
    {
        [TestMethod]
        public async Task SimpleValueDeleteAllTest()
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

            //Check we can delete it - no keys so delete all
            var deleted = await Repository.DeleteAll();
            Assert.IsTrue(deleted);

            //Check it's no longer there
            var any = await Repository.Any();
            Assert.IsTrue(!any);
        }

        [TestMethod]
        public async Task SimpleValueDeleteExpressionTest()
        {
            //Save one
            var id1 = Guid.NewGuid();
            var save1 = await Repository.Save(
                new SimpleKeyTestModel()
                {
                    Id = id1,
                    Date = DateTime.UtcNow - new TimeSpan(1, 0, 0, 0),
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save1);

            //Save another
            var id2 = Guid.NewGuid();
            var save2 = await Repository.Save(
                new SimpleKeyTestModel()
                {
                    Id = id2,
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
