using Repository.Abstraction;
using System.Linq.Expressions;

namespace SerialisedFile.Repository
{
    public partial class StorageFileRepository<TKey, TValue>
        : IRepositoryAny<TKey, TValue>
    {
        public Task<bool> Any(TKey key)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Any()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Any(Expression<Func<TValue, bool>> queryExpression, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            return Task.FromResult(FileReader.Any());
        }
    }
}
