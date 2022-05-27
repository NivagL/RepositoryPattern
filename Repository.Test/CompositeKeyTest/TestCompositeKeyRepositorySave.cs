using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Test.Model;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestCompositeKeyRepositorySave : TestCompositeKeyRepository
    {
        [TestMethod]
        public async Task CompositeKeySaveTest()
        {
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
        }

        [TestMethod]
        public async Task CompositeKeySaveValuesTest()
        {
            var id = Guid.NewGuid();
            var save = await Repository.Save(
                new CompositeKeyTestModel()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save);
        }
    }
}
