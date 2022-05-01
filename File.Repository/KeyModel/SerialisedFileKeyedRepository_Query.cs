using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SerialisedFile.Repository
{
    public partial class SerialisedFileKeyedRepository<TKey, TValue>
        : IKeyedRepositoryQuery<TKey, TValue>
    {
        public Task<KeyedPageResult<TKey, TValue>> KeyedPageQuery(Expression<Func<TValue, bool>> queryExpression, Expression<Func<TValue, object>> orderExpression, PageFilter pageFilter, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<TKey, TValue>> KeyedQuery(Expression<Func<TValue, bool>> queryExpression, Expression<Func<TValue, object>> orderExpression, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }
    }
}
