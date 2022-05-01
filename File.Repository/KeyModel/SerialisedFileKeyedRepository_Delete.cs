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
        : IKeyedRepositoryDelete<TKey, TValue>
    {
        public Task<TValue> KeyedDelete(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
