using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using Repository.Test.Model;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositorySave : TestSimpleKeyRepository
    {
        [TestMethod]
        public async Task SimpleKeySaveTest()
        {
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
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);
        }

        [TestMethod]
        public async Task SimpleValueSaveTest()
        {
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
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);
        }
    }
}
