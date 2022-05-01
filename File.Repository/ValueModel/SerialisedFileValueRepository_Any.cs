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
        : IValueRepositoryAny<TValue>
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
