using Repository.Abstraction;
using Repository.Files;

namespace SerialisedFile.Repository
{
    public partial class StorageFileRepository<TKey, TValue>
        : IRepository<TKey, TValue>
    {
        protected readonly string Directory;
        protected readonly IFileReader<TKey, TValue> FileReader;
        protected readonly IFileWriter<TKey, TValue> FileWriter;
        protected readonly IFileDeleter<TKey, TValue> FileDeleter;

        public StorageFileRepository(string directory
            , IFileReader<TKey, TValue> fileReader
            , IFileWriter<TKey, TValue> fileWriter
            , IFileDeleter<TKey, TValue> fileDeleter)
        {
            Directory = directory;
            FileReader = fileReader;
            FileWriter = fileWriter;
            FileDeleter = fileDeleter;
        }
    }
}
