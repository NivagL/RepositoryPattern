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
        : IValueRepositoryDelete<TValue>
    {
        public Task<bool> Delete(TValue value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteQuery(Expression<Func<TValue, bool>> queryExpression)
        {
            throw new NotImplementedException();
        }
    }
}
