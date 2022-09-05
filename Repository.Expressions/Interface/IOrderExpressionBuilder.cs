using System;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    /// <summary>
    /// Create an order by expression for TValue
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns>The expression</returns>
    public interface IOrderExpressionBuilder<TValue>
    {        
        Expression<Func<TValue, object>> CreateExpression(string propertyNames, char propertyDelimiter = '|');
    }
}
