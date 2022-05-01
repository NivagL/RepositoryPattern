using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestCompositeKeyRepositoryDelete : TestCompositeKeyRepository
    {
        [TestMethod]
        public async Task CompositeKeyDeleteTest()
        {
            //Save one
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

            //Check we can delete it
            var deleted = await Repository.KeyedDelete(Tuple.Create(id, date));
            Assert.IsTrue(deleted.Id == id);

            //Check it's no longer there
            try
            {
                var load = await Repository.KeyedLoad(Tuple.Create(id, date));
                Assert.Fail("Should throw");
            }
            catch (Exception)
            {
            }
        }
    }
}
