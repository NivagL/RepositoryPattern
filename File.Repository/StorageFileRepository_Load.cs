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
        : IRepositoryLoad<TKey, TValue>
    {
        public Task<TValue> Load(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<TKey, TValue>> LoadAll(PageSelection pageSelection, Expression<Func<TValue, object>> orderExpression, SortOrderEnum sortOrder = SortOrderEnum.Ascending)
        {
            throw new NotImplementedException();
        }
    }
}
