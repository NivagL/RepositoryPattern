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
        : IKeyedRepositoryLoad<TKey, TValue>
    {
        public Task<TValue> KeyedLoad(TKey key, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }

        public Task<KeyedPageResult<TKey, TValue>> KeyedLoadAll(PageSelection pageSelection, Expression<Func<TValue, object>> orderExpression, SortOrderEnum sortOrder = SortOrderEnum.Ascending, LoadFlagsEnum loadFlags = LoadFlagsEnum.All)
        {
            throw new NotImplementedException();
        }
    }
}
