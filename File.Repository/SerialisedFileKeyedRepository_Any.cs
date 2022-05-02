using Repository.Abstraction;
using System.Linq.Expressions;

namespace SerialisedFile.Repository
{
    public partial class SerialisedFileRepository<TKey, TValue>
        : IRepositoryAny<TKey, TValue>
    {
        public Task<bool> Any(Expression<Func<TValue, bool>> queryExpression, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any()
        {
            throw new NotImplementedException();
        }
    }
}
