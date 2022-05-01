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
        : IValueRepositoryLoad<TValue>
    {
        public Task<ValuePageResult<TValue>> LoadAll(PageSelection pageSelection, Expression<Func<TValue, object>> orderExpression, SortOrderEnum sortOrder = SortOrderEnum.Ascending, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }
    }
}
