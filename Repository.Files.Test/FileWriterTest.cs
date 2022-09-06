using Configuration.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Test.Model;
using System;

namespace Repository.Files.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class FileWriterTest
    {
        protected readonly UtilityBuilder UtilityBuilder;
        protected readonly IConfiguration Configuration;
        protected readonly ILoggerFactory LoggerFactory;
        protected readonly IServiceCollection Services;
        private readonly IFileWriter<Guid, SimpleKeyTestModel> FileWriter;

        public FileWriterTest()
        {
            UtilityBuilder = new UtilityBuilder();
            Configuration = UtilityBuilder.Configuration;
            Services = UtilityBuilder.Services;
            LoggerFactory = new LoggerFactory();

            FileWriter = new Files.FileWriter<Guid, SimpleKeyTestModel>(
                Configuration, LoggerFactory, 
                    new SimpleKeyTestModelMeta(), "");
        }

        [TestMethod]
        public void TestFileWriter()
        {
            var meta = new SimpleKeyTestModelMeta();
            var simpleTestModel = new SimpleKeyTestModel()
            {
                Id = meta.NewKey(),
                Date = System.DateTime.UtcNow,
                Processed = false,
                Description = "TestWritter"
            };
            var write = FileWriter.Write(simpleTestModel);
        }
    }
}