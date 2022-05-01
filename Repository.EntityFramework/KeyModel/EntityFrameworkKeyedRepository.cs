using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkKeyedRepository<TContext, TKey, TValue> 
    : EntityFrameworkValueRepository<TContext, TValue>
    , IKeyedRepository<TKey, TValue>
    where TContext : DbContext
    where TValue : class
    where TKey : notnull
{
    private readonly IKeyedModel<TKey, TValue> KeyedModel;

    public EntityFrameworkKeyedRepository(IConfiguration configuration, 
        ILogger<EntityFrameworkRepository> logger,
        TContext context, IKeyedModel<TKey, TValue> model,
        bool trackQueries = false)
        : base(configuration, logger, context, 
            model as IValueModel<TValue>)
    {
        KeyedModel = model;
    }

#pragma warning disable CS1998
    public virtual async Task LoadRelated(IDictionary<TKey, TValue> data, LoadFlagsEnum loadFlags)
    {
    }
#pragma warning restore CS1998
}
