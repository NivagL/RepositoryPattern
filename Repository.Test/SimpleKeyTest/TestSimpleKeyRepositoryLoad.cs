using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using Repository.Test.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class TestSimpleKeyRepositoryLoad : TestSimpleKeyRepository
    {
        [TestMethod]
        public async Task SimpleKeyLoadTest()
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

            //Check we can load it
            var load = await Repository.Load(id);
            Assert.IsTrue(load.Id == id);
        }

        //[TestMethod]
        //public async Task SimpleKeyPageLoadTest()
        //{
        //    //Save one
        //    var id = Guid.NewGuid();
        //    var save = await Repository.KeyedSave(
        //        new SimpleKeyTestModel()
        //        {
        //            Id = id,
        //            Date = DateTime.UtcNow,
        //            Description = "Test",
        //            Processed = false
        //        }
        //    );
        //    Assert.IsTrue(save.Item1 == id);

        //    //Check we can load it
        //    var load = await Repository.KeyedLoadAll(
        //        new PageSelection() { PageSize = 10, PageNumber = 1},
        //        x => x.Date
        //        );

        //    Assert.IsTrue(load.Data.ContainsKey(id));
        //}

        [TestMethod]
        public async Task SimpleValueLoadTest()
        {
            //Delete them
            var delete = await Repository.DeleteAll();

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

            //Check we can load it - no keys so load all
            var load = await Repository.LoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1 },
                x => x.Date
            );
            Assert.IsTrue(load.Data.Where(x => x.Id == id).Any());
        }

        [TestMethod]
        public async Task SimpleValuePageLoadTest()
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

            //Check we can load it
            var load = await Repository.LoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1 },
                x => x.Date
                );

            Assert.IsTrue(load.Data.Where(x => x.Id == id).Any());
        }
    }
}
