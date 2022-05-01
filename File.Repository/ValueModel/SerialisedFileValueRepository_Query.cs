using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SerialisedFile.Repository
{
    public partial class SerialisedFileValueRepository<TValue>
        : IValueRepositoryQuery<TValue>
    {
        public Task<IEnumerable<TValue>> FromSql(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<ValuePageResult<TValue>> PagedQuery(Expression<Func<TValue, bool>> queryExpression, Expression<Func<TValue, object>> orderExpression, PageFilter pageFilter, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TValue>> Query(Expression<Func<TValue, bool>> queryExpression, Expression<Func<TValue, object>> orderExpression, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }
    }
}
