using System;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    /// <summary>
    /// Create an order by expression for TValue
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns>The expression</returns>
    public interface IOrderExpression<TValue>
    {        
        Expression<Func<TValue, object>> Create(string propertyNames, char propertyDelimiter = '|');
    }
}
