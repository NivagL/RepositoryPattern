using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Test.Model;
using System;

namespace Repository.Files.Test
{
    [TestClass]
    [TestCategory("Integration")]
    public class FileWriterTest
    {
        private readonly IFileWriter<Guid, SimpleKeyTestModel> FileWriter;
        //var fileWritter = new FileWriter<Guid, SimpleKeyTestModel>();

        public FileWriterTest()
        {
            //FileWriter = new FileWriter()
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