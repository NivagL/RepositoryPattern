using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Structural")]
    public class InitialisationCompositKey : TestDatabase<CompositeKeyTestContext>
    {
        public InitialisationCompositKey()
            : base("CompositeKeyTest")
        {
        }

        [TestMethod]
        public async Task CreateCompositKeyDatabaseTest()
        {
            var provider = UtilityBuilder.Provider;
            var init = await DatabaseInitialiser.InitialiseDatabase(provider);
            Assert.IsTrue(init);
        }
    }
}
