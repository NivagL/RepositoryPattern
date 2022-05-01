using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
