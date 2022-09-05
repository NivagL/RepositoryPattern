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
        : IRepositoryQuery<TKey, TValue>
    {
        public Task<PageResult<TKey, TValue>> PagedQuery(Expression<Func<TValue, bool>> queryExpression, Expression<Func<TValue, object>> orderExpression, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }
    }
}
