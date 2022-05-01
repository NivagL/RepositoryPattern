using Microsoft.EntityFrameworkCore;
using Repository.Abstraction;

namespace EntityFramework.Repository;

public partial class EntityFrameworkKeyedRepository<TContext, TKey, TValue>
    : EntityFrameworkValueRepository<TContext, TValue>
    , IKeyedRepositoryAny<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
}
