using System;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    /// <summary>
    /// Create an order by expression for TValue
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IOrderExpressionBuilder<TValue>
    {        /// <returns></returns>
        Expression<Func<TValue, object>> CreateExpression(string propertyNames, char propertyDelimiter = '|');
    }
}
