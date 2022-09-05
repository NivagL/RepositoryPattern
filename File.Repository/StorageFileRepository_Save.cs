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
        : IRepositorySave<TKey, TValue>
    {
        public Task<IEnumerable<Tuple<TKey, ChangeEnum>>> Save(IEnumerable<TValue> values, bool replace = true, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<TKey, ChangeEnum>> Save(TValue value, bool replace = true, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }
    }
}
