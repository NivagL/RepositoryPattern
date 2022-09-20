using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    /// <summary>
    /// Generate an expression for a set of QueryObjects
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns>The expression</returns>
    public interface IQueryExpression<TValue>
    {
        Expression<Func<TValue, bool>> Create(IEnumerable<QueryObject> queryObjects);
    }
}
