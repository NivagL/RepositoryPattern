using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Abstraction;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFramework.Repository;

public partial class EntityFrameworkValueRepository<TContext, TValue> 
    : EntityFrameworkRepository
    , IValueRepository<TValue>
    where TContext : DbContext
    where TValue : class
{
    protected readonly DbSet<TValue> Set;
    protected readonly IValueModel<TValue> ValueModel;

    public EntityFrameworkValueRepository(IConfiguration configuration, 
        ILogger<EntityFrameworkRepository> logger,
        DbContext context, IValueModel<TValue> model,
        bool trackQueries = false)
        : base(configuration, logger, context, trackQueries)
    {
        ValueModel = model;
        Set = Context.Set<TValue>();
    }

    private void SaveChanges()
    {
        Context.SaveChanges();
    }

    public virtual IQueryable<TValue> Queryable(LoadFlagsEnum loadFlags)
    {
        return Context.Set<TValue>();
    }

    public virtual IQueryable<TValue> Queryable(
        Expression<Func<TValue, bool>> expression, 
        LoadFlagsEnum loadFlags)
    {
        return Context.Set<TValue>().Where(expression);
    }

    public virtual void LoadRelated(TValue value, LoadFlagsEnum loadFlags)
    {
    }

#pragma warning disable CS1998
    public virtual async Task LoadRelated(IEnumerable<TValue> data, LoadFlagsEnum loadFlags)
    {
    }
#pragma warning restore CS1998

    public virtual void RemoveRelated(TValue value)
    {
    }
}
