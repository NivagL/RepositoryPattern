﻿using Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SerialisedFile.Repository
{
    public partial class SerialisedFileKeyedRepository<TKey, TValue>
        : IKeyedRepositorySave<TKey, TValue>
    {
        public Task<Tuple<TKey, ChangeEnum>> KeyedSave(TValue value, bool replace = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<TKey, ChangeEnum>>> KeyedSaveAll(IEnumerable<TValue> values, bool replace = true)
        {
            throw new NotImplementedException();
        }
    }
}
