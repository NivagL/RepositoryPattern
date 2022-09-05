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
    public class TestCompositeKeyRepositoryLoad : TestCompositeKeyRepository
    {
        [TestMethod]
        public async Task CompositeKeyLoadTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var save = await Repository.Save(
                new CompositeKeyTestModel()
                {
                    Id = id,
                    Date = date,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can load it
            var load = await Repository.Load(Tuple.Create(id, date));
            Assert.IsTrue(load.Id == id);
        }

        //[TestMethod]
        //public async Task CompositeKeyPageLoadTest()
        //{
        //    //Save one
        //    var id = Guid.NewGuid();
        //    var date = DateTime.UtcNow;
        //    var save = await Repository.KeyedSave(
        //        new CompositeKeyTestModel()
        //        {
        //            Id = id,
        //            Date = date,
        //            Description = "Test",
        //            Processed = false
        //        }
        //    );
        //    Assert.IsTrue(save.Item1.Item1 == id
        //        && save.Item1.Item2 == date);

        //    //Check we can load it
        //    var load = await Repository.KeyedLoadAll(
        //        new PageSelection() { PageSize = 10, PageNumber = 1},
        //        x => x.Date
        //        );

        //    Assert.IsTrue(load.Data.ContainsKey(Tuple.Create(id, date)));
        //}

        [TestMethod]
        public async Task CompositKeyLoadValuesTest()
        {
            //Delete them
            var delete = await Repository.DeleteAll();

            //Save one
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
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can load it - no keys so load all
            var load = await Repository.LoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1 },
                x => x.Date
            );
            Assert.IsTrue(load.Data.Where(x => x.Value.Id == id).Any());
        }

        [TestMethod]
        public async Task CompositeKeyPageLoadValuesTest()
        {
            //Clean up
            var delete = await Repository.DeleteAll();

            //Save one
            var id = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var save = await Repository.Save(
                new CompositeKeyTestModel()
                {
                    Id = id,
                    Date = date,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can load it
            var load = await Repository.LoadAll(
                new PageSelection() { PageSize = 10, PageNumber = 1 },
                x => x.Date
                );

            Assert.IsTrue(load.Data.Where(x => x.Value.Id == id && x.Value.Date == date).Any());
        }
    }
}
