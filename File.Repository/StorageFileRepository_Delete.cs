using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SerialisedFile.Repository
{
    public partial class StorageFileRepository<TKey, TValue>
        : IRepositoryDelete<TKey, TValue>
    {
        public Task<TValue> Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteQuery(Expression<Func<TValue, bool>> queryExpression)
        {
            throw new NotImplementedException();
        }
    }
}
