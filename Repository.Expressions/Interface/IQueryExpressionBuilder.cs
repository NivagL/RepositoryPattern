using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Expressions
{
    /// <summary>
    /// Generate an expression for a set of QueryObjects
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IQueryExpressionBuilder<TValue> //: IExpressionBuilder<TValue>
    {
        Expression<Func<TValue, bool>> CreateExpression(IEnumerable<QueryObject> queryObjects);
    }
}
