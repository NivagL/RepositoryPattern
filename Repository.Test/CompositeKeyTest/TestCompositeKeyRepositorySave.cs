﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Abstraction;
using System;
using System.Threading.Tasks;
using Test.Model;

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
        }

        [TestMethod]
        public async Task CompositeKeySaveValuesTest()
        {
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
        }
    }
}
