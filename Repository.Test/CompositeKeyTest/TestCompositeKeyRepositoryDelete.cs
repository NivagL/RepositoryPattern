﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using System;
using System.Threading.Tasks;
using Test.Model;

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
            var save = await Repository.Save(
                new CompositeKeyModel()
                {
                    Id = id,
                    Date = date,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can delete it
            var deleted = await Repository.Delete(Tuple.Create(id, date));
            Assert.IsTrue(deleted.Id == id);

            //Check it's no longer there
            try
            {
                var load = await Repository.Load(Tuple.Create(id, date));
                Assert.Fail("Should throw");
            }
            catch (Exception)
            {
            }
        }

        [TestMethod]
        public async Task CompositKeyDeleteAllValuesTest()
        {
            //Save one
            var id = Guid.NewGuid();
            var save = await Repository.Save(
                new CompositeKeyModel()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save.Item2 == ChangeEnum.Added);

            //Check we can delete it - no keys so delete all
            var deleted = await Repository.DeleteAll();
            Assert.IsTrue(deleted > 0);

            //Check it's no longer there
            var any = await Repository.Any();
            Assert.IsTrue(!any);
        }

        [TestMethod]
        public async Task CompositeDeleteExpressionValuesTest()
        {
            //Save one
            var id1 = Guid.NewGuid();
            var save1 = await Repository.Save(
                new CompositeKeyModel()
                {
                    Id = id1,
                    Date = DateTime.UtcNow - new TimeSpan(1, 0, 0, 0),
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save1.Item2 == ChangeEnum.Added);

            //Save another
            var id2 = Guid.NewGuid();
            var save2 = await Repository.Save(
                new CompositeKeyModel()
                {
                    Id = id2,
                    Date = DateTime.UtcNow,
                    Description = "Test",
                    Processed = false
                }
            );
            Assert.IsTrue(save2.Item2 == ChangeEnum.Added);

            var earliest = DateTime.UtcNow - new TimeSpan(0, 1, 0, 0);
            //Check we can delete it - no keys so delete all
            var deleted = await Repository.DeleteQuery(
                x => x.Date < earliest);
            Assert.IsTrue(deleted > 0);

            //Check it's no longer there
            var any = await Repository.Any();
            Assert.IsTrue(any);
        }
    }
}
