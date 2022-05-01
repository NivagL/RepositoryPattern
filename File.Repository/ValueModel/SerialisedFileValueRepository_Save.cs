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
        : IValueRepositorySave<TValue>
    {
        public Task<bool> Save(TValue value, bool replace = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAll(IEnumerable<TValue> values, bool replace = true)
        {
            throw new NotImplementedException();
        }
    }
}
