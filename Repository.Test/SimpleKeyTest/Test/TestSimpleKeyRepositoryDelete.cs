using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositoryDelete : TestSimpleKeyRepository
    {
        [TestMethod]
        public async Task SimpleKeyDeleteTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var save = await Repository.KeyedSave(
                new SimpleKeyTestModel()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item1 == id);

            //Check we can delete it
            var deleted = await Repository.KeyedDelete(id);
            Assert.IsTrue(deleted.Id == id);

            //Check it's no longer there
            try
            {
                var load = await Repository.KeyedLoad(id);
                Assert.Fail("Should throw");
            }
            catch (Exception)
            {
            }
        }
    }

   }
