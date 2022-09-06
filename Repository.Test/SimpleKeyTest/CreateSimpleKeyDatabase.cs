using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Structural")]
    public class CreateSimpleKeyDatabase : TestDatabase<SimpleKeyTestContext>
    {
        public CreateSimpleKeyDatabase()
            : base("SimpleKeyTest")
        {
        }

        [TestMethod]
        public async Task CreateSimpleKeyDatabaseTest()
        {
            var provider = UtilityBuilder.Provider;
            var init = await DatabaseInitialiser.InitialiseDatabase(provider);
            Assert.IsTrue(init);
        }
    }
}
