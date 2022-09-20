using Microsoft.Extensions.DependencyInjection;
using System;

namespace Repository.Expressions
{
    public interface IExpressionsFactory<TValue>
    {
        Func<IServiceProvider, IOrderExpression<TValue>> OrderExpressionBuilder { get; set; }
        Func<IServiceProvider, IQueryExpression<TValue>> QueryExpressionBuilder { get; set; }
        void RegisterTypes(IServiceCollection services);
    }
}
