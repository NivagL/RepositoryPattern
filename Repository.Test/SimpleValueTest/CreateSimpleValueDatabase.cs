using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Model;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Structural")]
    public class CreateSimpleValueDatabase : TestDatabase<SimpleValueTestContext>
    {
        public CreateSimpleValueDatabase()
            : base("SimpleValueTest")
        {
        }

        [TestMethod]
        public async Task CreateSimpleValueDatabaseTest()
        {
            var provider = DependencyBuilder.Provider;
            var init = await DatabaseInitialiser.InitialiseDatabase(provider);
            Assert.IsTrue(init);
        }
    }
}
