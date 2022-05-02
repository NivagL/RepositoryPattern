using Repository.Abstraction;
using Repository.Files;

namespace SerialisedFile.Repository
{
    public partial class SerialisedFileRepository<TKey, TValue>
        : IRepository<TKey, TValue>
    {
        private readonly IKeyFileReader<TKey, TValue> FileReader;
        private readonly IKeyFileWriter<TKey, TValue> FileWriter;
        private readonly IKeyFileDeleter<TKey, TValue> FileDeleter;

        public SerialisedFileRepository(
            IKeyFileReader<TKey, TValue> fileReader
            , IKeyFileWriter<TKey, TValue> fileWriter
            , IKeyFileDeleter<TKey, TValue> fileDeleter)
        {
            FileReader = fileReader;
            FileWriter = fileWriter;
            FileDeleter = fileDeleter;
        }
    }
}
