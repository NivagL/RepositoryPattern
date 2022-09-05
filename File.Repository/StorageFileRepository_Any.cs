using Repository.Abstraction;
using System.Linq.Expressions;

namespace SerialisedFile.Repository
{
    public partial class StorageFileRepository<TKey, TValue>
        : IRepositoryAny<TKey, TValue>
    {
        public Task<bool> Any()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<TValue, bool>> queryExpression)
        {
            throw new NotImplementedException();
        }
    }
}
