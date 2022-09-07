using Microsoft.Extensions.DependencyInjection;
using System;

namespace Repository.Expressions
{
    public interface IRepositoryExpressionsFactory<TValue>
    {
        Func<IServiceProvider, IOrderExpressionBuilder<TValue>> OrderExpressionBuilder { get; set; }
        Func<IServiceProvider, IQueryExpressionBuilder<TValue>> QueryExpressionBuilder { get; set; }
        void RegisterTypes(IServiceCollection services);
    }
}
